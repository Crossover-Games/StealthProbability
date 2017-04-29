using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelBuilderRemake {
	/// <summary>
	/// Builds a level from a level blueprint.
	/// </summary>
	public class LevelAssembler : ScriptableObject {
		public GameObject floorTilePrefab;
		public GameObject wallTilePrefab;
		public GameObject victoryTilePrefab;
		public GameObject gameControllerPrefab;
		public GameObject catPrefab;
		public GameObject dogPrefab;
		public GameObject pathPrefab;
		public GameObject routePrefab;

		public static void AssembleLevel (LevelBlueprint blueprint) {
			LevelAssembler assembler = ScriptableObject.CreateInstance<LevelAssembler> ();

			GameObject found = GameObject.FindGameObjectWithTag ("GameController");
			if (found == null) {
				GameObject g = PrefabUtility.InstantiatePrefab (assembler.gameControllerPrefab) as GameObject;
				g.name = "GameController";
			}

			found = GameObject.FindGameObjectWithTag ("MapTilesParent");
			if (found != null) {
				DestroyImmediate (found);
			}
			found = new GameObject ("Map Tiles");
			found.tag = "MapTilesParent";
			for (int i = 0; i < blueprint.tiles.GetLength (0); i++) {
				for (int j = 0; j < blueprint.tiles.GetLength (1); j++) {
					Point2D thisPoint = new Point2D (i, j);
					if (blueprint.victoryTiles.Contains (thisPoint)) {
						assembler.InstantiateVictoryTile (thisPoint.ToVector3XZ (), found.transform);
					}
					else if (blueprint.tiles [i, j]) {
						assembler.InstantiateFloor (thisPoint.ToVector3XZ (), found.transform);
					}
					else {
						assembler.InstantiateWall (thisPoint.ToVector3XZ (), found.transform);
					}
				}
			}

			// finds the actual cat parent, then destroys all existing cats
			GameObject catParent = GameObject.FindGameObjectWithTag ("CatParent");
			List<GameObject> forgottenChildren = new List<GameObject> ();
			foreach (Transform child in catParent.transform) {
				forgottenChildren.Add (child.gameObject);
			}
			forgottenChildren.ForEach (child => DestroyImmediate (child));

			foreach (CatBlueprint chara in blueprint.cats) {
				GameObject g = PrefabUtility.InstantiatePrefab (assembler.catPrefab) as GameObject;
				g.transform.position = chara.location.ToVector3XZ () + Vector3.up * 0.5f;
				g.transform.rotation = Compass.DirectionToRotation (chara.orientation);
				g.name = chara.characterName;
				g.transform.SetParent (catParent.transform, true);

				Cat c = g.GetComponent<Cat> ();
				c.orientation = chara.orientation;
				c.SetSerializedIntProperty ("m_maxEnergy", chara.energy);
			}

			// finds the actual dog parent, then destroys all existing dogs
			GameObject dogParent = GameObject.FindGameObjectWithTag ("DogParent");
			forgottenChildren = new List<GameObject> ();
			foreach (Transform child in dogParent.transform) {
				forgottenChildren.Add (child.gameObject);
			}
			forgottenChildren.ForEach (child => DestroyImmediate (child));

			found = GameObject.FindGameObjectWithTag ("RouteParent");
			if (found != null) {
				DestroyImmediate (found);
			}
			GameObject routeParent = new GameObject ("Routes");
			routeParent.tag = "RouteParent";

			foreach (DogBlueprint chara in blueprint.dogs) {
				GameObject g = PrefabUtility.InstantiatePrefab (assembler.dogPrefab) as GameObject;
				g.transform.position = chara.location.ToVector3XZ () + Vector3.up * 0.5f;
				g.transform.rotation = Compass.DirectionToRotation (chara.orientation);
				g.name = chara.characterName;
				g.transform.SetParent (dogParent.transform, true);

				Dog d = g.GetComponent<Dog> ();
				d.visionType = chara.visionType;
				d.orientation = chara.orientation;
				assembler.ConstructPath (chara, d, routeParent);
			}
		}

		private void ConstructPath (DogBlueprint dogBlueprint, Dog dog, GameObject routeParent) {
			HashSet<StepNode> normalNodes = new HashSet<StepNode> ();
			HashSet<StepNode> stopNodes = new HashSet<StepNode> ();
			HashSet<StepNode> allNodes = new HashSet<StepNode> ();

			GameObject routeObject = GameObject.Instantiate (routePrefab);
			routeObject.transform.SetParent (routeParent.transform);

			Route route = routeObject.GetComponent<Route> ();
			route.SetSerializedReferenceProperty ("m_dog", dog);
			dog.SetSerializedReferenceProperty ("m_route", route);
			routeObject.name = dog.name + "'s Route";


			for (int i = 0; i < dogBlueprint.nodeMap.GetLength (0); i++) {
				for (int j = 0; j < dogBlueprint.nodeMap.GetLength (1); j++) {
					if ((dogBlueprint.location.x == i && dogBlueprint.location.z == j) || dogBlueprint.nodeMap [i, j] != PathNodeState.Empty) {
						RaycastHit hit;
						if (XZRaycast (i, j, out hit)) {
							StepNode existingNode = hit.collider.GetComponent<StepNode> ();
							if (existingNode != null) {
								DestroyImmediate (existingNode);
							}
							StepNode newNode = hit.collider.gameObject.AddComponent<StepNode> ();

							allNodes.Add (newNode);
							if ((dogBlueprint.location.x == i && dogBlueprint.location.z == j) || dogBlueprint.nodeMap [i, j] == PathNodeState.StopNode) {
								stopNodes.Add (newNode);
								newNode.SetStoppingPoint (true);
							}
							else if (dogBlueprint.nodeMap [i, j] == PathNodeState.NormalNode) {
								normalNodes.Add (newNode);
								newNode.SetStoppingPoint (false);
							}
						}
					}
				}
			}
			List<Path> allPaths = new List<Path> ();

			foreach (StepNode sn in allNodes) {
				RegisterConnections (sn, allNodes);
			}

			List<Path> currentPathsOnRoute = new List<Path> ();
			foreach (StepNode currentStop in stopNodes) {
				foreach (StepNode secondOnPath in GetConnections (currentStop, allNodes)) {
					StepNode beforeEndpoint;
					List<StepNode> registerToPath;
					StepNode endpoint = FollowToEnd (currentStop, secondOnPath, allNodes, out beforeEndpoint, out registerToPath);

					GameObject pathObject = GameObject.Instantiate (pathPrefab);
					Path tempPath = pathObject.GetComponent<Path> ();
					tempPath.DefinePath (currentStop, secondOnPath, endpoint, beforeEndpoint);

					if (!currentPathsOnRoute.Exists ((Path p) => p.Equivalent (tempPath))) {
						currentPathsOnRoute.Add (tempPath);
						pathObject.transform.SetParent (routeObject.transform);
						route.AddPath (tempPath);
						foreach (StepNode toPath in registerToPath) {
							toPath.SetSerializedReferenceProperty ("m_path", tempPath);
							Debug.Log (toPath.myPath);
						}
					}
					else {
						currentPathsOnRoute.Remove (tempPath);
						DestroyImmediate (tempPath.gameObject);
					}

				}
			}
			/*while (stopNodes.Count > 0) {
				StepNode start = stopNodes.FirstElement ();
				foreach (StepNode secondOnPath in GetConnections (start, allNodes)) {
					StepNode beforeEndpoint;
					List<StepNode> registerToPath;
					StepNode endpoint = FollowToEnd (start, secondOnPath, allNodes, out beforeEndpoint, out registerToPath);

					if (!stopNodes.Contains (endpoint)) {
						GameObject pathObject = GameObject.Instantiate (pathPrefab);
						Path tempPath = pathObject.GetComponent<Path> ();
						route.AddPath (tempPath);
						pathObject.transform.SetParent (routeObject.transform);
						foreach (StepNode toPath in registerToPath) {
							toPath.SetSerializedReferenceProperty ("m_path", tempPath);
						}
						tempPath.DefinePath (start, secondOnPath, endpoint, beforeEndpoint);
					}
				}
				stopNodes.Remove (start);
			} */
		}

		/// <summary>
		/// ha, where have you seen this before?
		/// </summary>
		private static StepNode NextOnPath (StepNode examining, StepNode cameFrom, ICollection<StepNode> valid) {
			if (examining.isStoppingPoint) {
				return null;
			}
			else {
				HashSet<StepNode> remainingNodes = new HashSet<StepNode> (GetConnections (examining, valid));
				remainingNodes.Remove (cameFrom);

				if (remainingNodes.Count == 1) {
					return remainingNodes.ToArray () [0];
				}
				else if (remainingNodes.Count == 0) {
					return cameFrom;
				}
				else {
					return null;
				}
			}
		}

		/// <summary>
		/// Uses raycasts to get the connections of a node
		/// </summary>
		private static List<StepNode> GetConnections (StepNode a, ICollection<StepNode> valid) {
			List<StepNode> connections = new List<StepNode> ();
			foreach (Tile t in AllNeighbors (a.GetComponent<Tile> ())) {
				StepNode other = t.GetComponent<StepNode> ();
				if (valid.Contains (other)) {
					connections.Add (other);
				}
			}
			return connections;
		}


		/// <summary>
		/// Returns all neighbors.
		/// </summary>
		private static List<Tile> AllNeighbors (Tile t, bool includeDiagonals = false) {
			List<Tile> tempList = new List<Tile> ();
			RaycastHit hit;
			for (int x = -1; x <= 1; x++) {
				for (int z = -1; z <= 1; z++) {
					if (!(x == 0 && z == 0)) {
						if (includeDiagonals || Utility.AbsInt (x) != Utility.AbsInt (z)) {
							if (XZRaycast (t.transform.position.x + x, t.transform.position.z + z, out hit)) {
								Tile tileTemp = hit.collider.gameObject.GetComponent<Tile> ();
								if (tileTemp != null) {
									tempList.Add (tileTemp);
								}
							}
						}
					}
				}
			}
			return tempList;
		}

		/// <summary>
		/// Registers the serialized connections to all neighbors, given a set of valid nodes.
		/// </summary>
		private static void RegisterConnections (StepNode a, ICollection<StepNode> valid) {
			foreach (StepNode connection in GetConnections (a, valid)) {
				a.RegisterConnection (connection);
			}
		}

		/// <summary>
		/// Traces a route to the end. Returns the endpoint.
		/// </summary>
		private static StepNode FollowToEnd (StepNode start, StepNode next, ICollection<StepNode> valid, out StepNode beforeEndpoint, out List<StepNode> registerToPath) {
			List<StepNode> steps = new List<StepNode> ();
			StepNode previous = start;
			StepNode current = next;
			while (current != null) {
				steps.Add (current);
				StepNode tempPrevious = current;
				current = NextOnPath (current, previous, valid);
				previous = tempPrevious;
			}

			steps.RemoveAt (steps.Count - 1);
			beforeEndpoint = steps.LastElement ();
			registerToPath = steps;

			return previous;
		}

		private static bool XZRaycast (float x, float z, out RaycastHit hit) {
			return Physics.Raycast (new Vector3 (x, -100f, z), Vector3.up, out hit);
		}

		private void InstantiateFloor (Vector3 pos, Transform mapTilesParent) {
			GameObject g = PrefabUtility.InstantiatePrefab (floorTilePrefab) as GameObject;
			g.transform.position = pos;
			g.transform.SetParent (mapTilesParent);
		}
		private void InstantiateWall (Vector3 pos, Transform mapTilesParent) {
			GameObject g = PrefabUtility.InstantiatePrefab (wallTilePrefab) as GameObject;
			g.transform.position = pos;
			g.transform.SetParent (mapTilesParent);
		}
		private void InstantiateVictoryTile (Vector3 pos, Transform mapTilesParent) {
			GameObject g = PrefabUtility.InstantiatePrefab (victoryTilePrefab) as GameObject;
			g.transform.position = pos;
			g.transform.SetParent (mapTilesParent);
		}

		/// <summary>
		/// Updates the level blueprint based on the physical level.
		/// </summary>
		public static LevelBlueprint ScanLevel () {
			GameObject mapTilesParent = GameObject.FindGameObjectWithTag ("MapTilesParent");
			List<Tile> allTiles = new List<Tile> (mapTilesParent.GetComponentsInChildren<Tile> ());

			int minX = 9999;
			int maxX = -9999;
			int minZ = 9999;
			int maxZ = -9999;

			foreach (Tile t in allTiles) {
				if (Mathf.RoundToInt (t.transform.position.x) > maxX) {
					maxX = Mathf.RoundToInt (t.transform.position.x);
				}
				if (Mathf.RoundToInt (t.transform.position.x) < minX) {
					minX = Mathf.RoundToInt (t.transform.position.x);
				}
				if (Mathf.RoundToInt (t.transform.position.z) > maxZ) {
					maxZ = Mathf.RoundToInt (t.transform.position.z);
				}
				if (Mathf.RoundToInt (t.transform.position.z) < minZ) {
					minZ = Mathf.RoundToInt (t.transform.position.z);
				}
			}

			LevelBlueprint blueprint = LevelBlueprint.DefaultLevel ();

			blueprint.widthDisplay = maxX - minX + 1;
			blueprint.lengthDisplay = maxZ - minZ + 1;
			blueprint.tiles.Set2DShallow (new bool [blueprint.widthDisplay, blueprint.lengthDisplay]);

			blueprint.victoryTiles = new List<Point2D> ();

			for (int x = minX; x <= maxX; x++) {
				for (int z = minZ; z <= maxZ; z++) {
					RaycastHit hit;
					if (XZRaycast (x, z, out hit)) {
						Tile tileTemp = hit.collider.gameObject.GetComponent<Tile> ();
						if (hit.collider.gameObject.GetComponent<VictoryTile> () != null) {
							blueprint.tiles [x, z] = true;
							blueprint.victoryTiles.Add (new Point2D (x, z));
						}
						else if (tileTemp != null) {
							blueprint.tiles [x, z] = tileTemp.traversable;
						}
						else {
							blueprint.tiles [x, z] = false;
						}
					}
					else {
						blueprint.tiles [x, z] = false;
					}
				}
			}

			blueprint.dogs = new List<DogBlueprint> ();
			foreach (Dog d in FindObjectsOfType<Dog> ()) {
				Point2D point = Point2D.FromTransformXZ (d.transform);
				DogBlueprint newlyCreatedBP = DogBlueprint.CreateDogBlueprint (d.name, d.orientation, point, d.visionType, blueprint);
				blueprint.dogs.Add (newlyCreatedBP);

				Route r = d.GetSerializedReferenceProperty<Route> ("m_route");
				SerializedProperty allPathsProperty = new UnityEditor.SerializedObject (r).FindProperty ("allPaths");
				int allPathsMax = allPathsProperty.arraySize;
				for (int x = 0; x < allPathsMax; x++) {
					//iterate through each path
					Path p = allPathsProperty.GetArrayElementAtIndex (x).objectReferenceValue as Path;
					StepNode endpoint = p.GetSerializedReferenceProperty<StepNode> ("endpointA");
					point = Point2D.FromTransformXZ (endpoint.transform);
					newlyCreatedBP.nodeMap [point.x, point.z] = PathNodeState.StopNode;
					endpoint = p.GetSerializedReferenceProperty<StepNode> ("endpointB");
					point = Point2D.FromTransformXZ (endpoint.transform);
					newlyCreatedBP.nodeMap [point.x, point.z] = PathNodeState.StopNode;
					foreach (StepNode sn in mapTilesParent.GetComponentsInChildren<StepNode> ()) {
						if (sn.myPath == p) {
							point = Point2D.FromTransformXZ (sn.transform);
							newlyCreatedBP.nodeMap [point.x, point.z] = PathNodeState.NormalNode;
						}
					}
				}
			}

			blueprint.cats = new List<CatBlueprint> ();
			foreach (Cat c in FindObjectsOfType<Cat> ()) {
				Point2D point = Point2D.FromTransformXZ (c.transform);
				CatBlueprint newlyCreatedBP = CatBlueprint.CreateCatBlueprint (c.name, c.orientation, point);
				blueprint.cats.Add (newlyCreatedBP);
			}
			return blueprint;
		}
	}
}