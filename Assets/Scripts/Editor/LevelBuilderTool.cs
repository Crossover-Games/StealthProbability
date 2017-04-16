using UnityEditor;
using UnityEngine;

using System.Collections.Generic;

namespace LevelBuilder {
	public partial class LevelBuilderTool : EditorWindow {

		public Texture floorImage;
		public Texture wallImage;
		public Texture normalNodeImage;
		public Texture stopNodeImage;
		public Texture dogOriginImage;

		public GameObject floorTile;
		public GameObject wallTile;
		public GameObject gameController;
		public GameObject dogPrefab;

		public GameObject pathPrefab;
		public GameObject routePrefab;

		// Add menu item to the upper bar
		[MenuItem ("Stealth/Level Builder")]
		public static void ShowWindow () {
			//Show existing window instance. If one doesn't exist, make one.
			EditorWindow.GetWindow (typeof (LevelBuilderTool));
		}

		private bool [,] fieldsArray = new bool [0, 0];
		private int length = 10;
		private int width = 10;
		private int lengthDisplay = 10;
		private int widthDisplay = 10;


		private bool extraSettingsFoldout = false;
		private bool expandedFloorDefault = true;
		private int originX = 0;
		private int originZ = 0;

		private bool tileImportExportFoldout = false;
		private string tileMetaText;

		private bool dogImportExportFoldout = false;
		private string dogMetaText;

		private bool dogListFoldout = false;
		private List<DogBlueprint> dogList;

		private GameObject mapTilesParent;
		private GameObject dogParent;

		private Vector2 levelScrollPos;
		private Vector2 dogScrollPos;

		private bool startup = true;

		/// <summary>
		/// Expands the array.
		/// </summary>
		private void ExpandArray () {
			length = lengthDisplay;
			width = widthDisplay;

			bool [,] newFieldsArray = new bool [width, length];
			PathNodeState [,] newPathArray = new PathNodeState [width, length];

			// array defaults
			for (int j = 0; j < newFieldsArray.GetLength (1); j++) {
				for (int i = 0; i < newFieldsArray.GetLength (0); i++) {
					newFieldsArray [i, j] = expandedFloorDefault;
					newPathArray [i, j] = expandedFloorDefault ? PathNodeState.Empty : PathNodeState.Wall;
				}
			}

			// copies old array, makes sure it is at least as big as the new array
			for (int j = 0; j < Mathf.Min (fieldsArray.GetLength (1), length); j++) {
				for (int i = 0; i < Mathf.Min (fieldsArray.GetLength (0), width); i++) {
					newFieldsArray [i, j] = fieldsArray [i, j];
				}
			}
			// copies the path arrays
			foreach (DogBlueprint dbp in dogList) {
				for (int j = 0; j < Mathf.Min (dbp.nodeMap.GetLength (1), length); j++) {
					for (int i = 0; i < Mathf.Min (dbp.nodeMap.GetLength (0), width); i++) {
						newPathArray [i, j] = dbp.nodeMap [i, j];
					}
				}
				dbp.nodeMap = newPathArray;
			}
			fieldsArray = newFieldsArray;
		}

		/// <summary>
		/// Updates the physical representation of the array.
		/// </summary>
		private void DrawFieldsArray () {
			for (int j = 0; j < length; j++) {
				EditorGUILayout.BeginHorizontal ();
				for (int i = 0; i < width; i++) {
					fieldsArray [i, j] = EditorGUILayout.Toggle (fieldsArray [i, j], GUILayout.ExpandWidth (false), GUILayout.Width (15f));
				}
				EditorGUILayout.EndHorizontal ();
			}
		}

		private void InstantiateFloor (Vector3 pos) {
			GameObject g = PrefabUtility.InstantiatePrefab (floorTile) as GameObject;
			g.transform.position = pos;
			g.transform.SetParent (mapTilesParent.transform);
		}
		private void InstantiateWall (Vector3 pos) {
			GameObject g = PrefabUtility.InstantiatePrefab (wallTile) as GameObject;
			g.transform.position = pos;
			g.transform.SetParent (mapTilesParent.transform);
		}
		/// <summary>
		/// Also registers the created dog to the blueprint.
		/// </summary>
		private void InstantiateDog (DogBlueprint dbp) {
			GameObject g = PrefabUtility.InstantiatePrefab (dogPrefab) as GameObject;
			Point2D g2w = GridToWorld (dbp.point.x, dbp.point.y);
			g.transform.position = new Vector3 (g2w.x, 0.5f, g2w.y);
			g.transform.rotation = Compass.DirectionToRotation (dbp.direction);
			g.transform.SetParent (dogParent.transform);
			Dog temp = g.GetComponent<Dog> ();
			temp.orientation = dbp.direction;
			dbp.myDog = temp;
		}

		/// <summary>
		/// Updates the level display based on the physical level.
		/// </summary>
		private void ScanLevel () {
			List<Tile> allTiles = new List<Tile> (FindObjectsOfType<Tile> ());
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

			widthDisplay = maxX - minX + 1;
			lengthDisplay = maxZ - minZ + 1;
			ExpandArray ();

			for (int x = minX; x <= maxX; x++) {
				for (int z = minZ; z <= maxZ; z++) {
					Point2D gridPoint = GridToWorld (x - minX, z - minZ);
					RaycastHit hit;
					if (TileRaycastHelper (x, z, out hit)) {
						Tile tileTemp = hit.collider.gameObject.GetComponent<Tile> ();
						if (tileTemp != null) {
							fieldsArray [gridPoint.x, gridPoint.y] = tileTemp.traversable;
						}
						else {
							fieldsArray [gridPoint.x, gridPoint.y] = false;
						}
					}
					else {
						fieldsArray [gridPoint.x, gridPoint.y] = false;
					}
				}
			}

			dogList = new List<DogBlueprint> ();
			foreach (Dog d in FindObjectsOfType<Dog> ()) {
				dogList.Add (new DogBlueprint (d.name, d.orientation, Mathf.RoundToInt (d.transform.position.x), Mathf.RoundToInt (d.transform.position.z)));
			}
			DrawFieldsArray ();
			ExportLevelTiles ();
		}

		/// <summary>
		/// Uses a raycast to find the object at the specified XZ position.
		/// </summary>
		private GameObject GameObjectAtXZ (float x, float z) {
			RaycastHit hit;
			if (TileRaycastHelper (x, z, out hit)) {
				return hit.collider.gameObject;
			}
			else {
				return null;
			}

		}
		private bool TileRaycastHelper (float x, float z, out RaycastHit hit) {
			return Physics.Raycast (new Vector3 (x, -100f, z), Vector3.up, out hit);
		}
		private bool TileRaycastHelper (Vector3 rayOrigin, out RaycastHit hit) {
			return Physics.Raycast (rayOrigin, Vector3.up, out hit);
		}

		/// <summary>
		/// Returns all neighbors.
		/// </summary>
		private List<Tile> AllNeighbors (Tile t, bool includeDiagonals = false) {
			List<Tile> tempList = new List<Tile> ();
			RaycastHit hit;
			for (int x = -1; x <= 1; x++) {
				for (int z = -1; z <= 1; z++) {
					if (!(x == 0 && z == 0)) {
						if (includeDiagonals || Utility.AbsInt (x) != Utility.AbsInt (z)) {
							if (TileRaycastHelper (t.transform.position.x + x, t.transform.position.z + z, out hit)) {
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
		/// Works both ways. Translates a grid point to a world point.
		/// </summary>
		private Point2D GridToWorld (int x, int z) {
			return new Point2D (x, length - z - 1);
		}
	}
}