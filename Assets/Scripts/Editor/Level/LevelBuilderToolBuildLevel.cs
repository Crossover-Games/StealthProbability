using UnityEditor;
using UnityEngine;

using System.Collections.Generic;

namespace LevelBuilder {
	public partial class LevelBuilderTool : EditorWindow {
		private void BuildLevel () {
			GameObject found = GameObject.FindGameObjectWithTag ("GameController");
			if (found == null) {
				GameObject g = PrefabUtility.InstantiatePrefab (gameController) as GameObject;
				g.name = "GameController";
			}

			found = GameObject.FindGameObjectWithTag ("MapTilesParent");
			if (found != null) {
				DestroyImmediate (found);
			}

			mapTilesParent = new GameObject ("Map Tiles");
			mapTilesParent.tag = "MapTilesParent";

			for (int j = length - 1; j >= 0; j--) {
				for (int i = 0; i < width; i++) {
					Point2D g2w = GridToWorld (i, j);
					Vector3 worldPos = new Vector3 (g2w.x, 0f, g2w.z);
					if (fieldsArray [i, j]) {
						InstantiateFloor (worldPos);
					}
					else {
						InstantiateWall (worldPos);
					}
				}
			}

			// finds the actual dog parent, then destroys all existing dogs
			dogParent = GameObject.FindGameObjectWithTag ("DogParent");
			List<GameObject> dogChildren = new List<GameObject> ();
			foreach (Transform child in dogParent.transform) {
				dogChildren.Add (child.gameObject);
			}
			dogChildren.ForEach (child => DestroyImmediate (child));


			found = GameObject.FindGameObjectWithTag ("RouteParent");
			if (found != null) {
				DestroyImmediate (found);
			}
			GameObject routeParent = new GameObject ("Routes");
			routeParent.tag = "RouteParent";

			HashSet<Point2D> points = new HashSet<Point2D> ();
			foreach (DogBlueprint dbp in dogList) {
				if (!points.Contains (dbp.point)) {
					InstantiateDog (dbp);
					points.Add (dbp.point);
					ConstructPath (dbp, routeParent);
				}
			}

			ExportLevelTiles ();
		}

		/// <summary>
		/// ha, where have you seen this before?
		/// </summary>
		private StepNode NextOnPath (StepNode examining, StepNode cameFrom, ICollection<StepNode> valid) {
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
		private List<StepNode> GetConnections (StepNode a, ICollection<StepNode> valid) {
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
		/// Registers the serialized connections to all neighbors, given a set of valid nodes.
		/// </summary>
		private void RegisterConnections (StepNode a, ICollection<StepNode> valid) {
			foreach (StepNode connection in GetConnections (a, valid)) {
				a.RegisterConnection (connection);
			}
		}

		/// <summary>
		/// Traces a route to the end. Returns the endpoint.
		/// </summary>
		private StepNode FollowToEnd (StepNode start, StepNode next, ICollection<StepNode> valid, out StepNode beforeEndpoint, out List<StepNode> registerToPath) {
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

		private void ConstructPath (DogBlueprint dbp, GameObject routeParent) {
			HashSet<StepNode> normalNodes = new HashSet<StepNode> ();
			HashSet<StepNode> stopNodes = new HashSet<StepNode> ();
			HashSet<StepNode> allNodes = new HashSet<StepNode> ();

			GameObject routeObject = GameObject.Instantiate (routePrefab);
			routeObject.transform.SetParent (routeParent.transform);

			Route route = routeObject.GetComponent<Route> ();
			route.SetSerializedDog (dbp.myDog);

			dbp.myDog.SetSerializedReferenceProperty ("m_route", route);

			routeObject.name = dbp.myDog.name + "'s Route";

			//Add nodes
			for (int j = length - 1; j >= 0; j--) {
				for (int i = 0; i < width; i++) {
					if ((dbp.point.x == i && dbp.point.z == j) || dbp.nodeMap [i, j] == PathNodeState.NormalNode || dbp.nodeMap [i, j] == PathNodeState.StopNode) {
						Point2D g2w = GridToWorld (i, j);
						RaycastHit hit;
						if (TileRaycastHelper (g2w.x, g2w.z, out hit)) {
							StepNode existingNode = hit.collider.GetComponent<StepNode> ();
							if (existingNode != null) {
								DestroyImmediate (existingNode);
							}
							StepNode newNode = hit.collider.gameObject.AddComponent<StepNode> ();

							allNodes.Add (newNode);
							if ((dbp.point.x == i && dbp.point.z == j) || dbp.nodeMap [i, j] == PathNodeState.StopNode) {
								stopNodes.Add (newNode);
								newNode.SetStoppingPoint (true);
							}
							else if (dbp.nodeMap [i, j] == PathNodeState.NormalNode) {
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

			while (stopNodes.Count > 0) {
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
			}
		}
	}
}