
using UnityEditor;
using UnityEngine;

public class LevelPrototypingTool : EditorWindow {
	
	// Add menu item to the upper bar
	[MenuItem ("Stealth/Level Prototyping Tool")]
	public static void ShowWindow () {
		ExpandArray ();
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow (typeof(LevelPrototypingTool));
	}

	private static bool[,] fieldsArray = new bool[0, 0];
	private static int length = 10;
	private static int width = 10;
	private static int lengthDisplay = 10;
	private static int widthDisplay = 10;

	private static bool extraSettings = false;
	private static bool expandedFloorDefault = true;
	private static int originX = 0;
	private static int originZ = 0;

	private static GameObject mapTilesParent;

	void OnGUI () {
		extraSettings = EditorGUILayout.Foldout (extraSettings, "Additional Settings", true);
		if (extraSettings) {
			expandedFloorDefault = EditorGUILayout.Toggle (new GUIContent ("New tiles floors by default", "After expanding the dimensions of the level, will new tiles be floors or walls?"), expandedFloorDefault);
			originX = EditorGUILayout.IntField (new GUIContent ("Origin X", "X value of top left corner. Integers only. No functional reason to modify."), originX);
			originZ = EditorGUILayout.IntField (new GUIContent ("Origin Z", "Z value of top left corner. Integers only. No functional reason to modify."), originZ);
		}


		GUILayout.Label ("Level Dimensions", EditorStyles.boldLabel);

		lengthDisplay = Mathf.Clamp (EditorGUILayout.IntField (new GUIContent ("Length", "Z dimension size of the level, in tiles"), lengthDisplay), 1, 500);
		widthDisplay = Mathf.Clamp (EditorGUILayout.IntField (new GUIContent ("Width", "X dimension size of the level, in tiles"), widthDisplay), 1, 500);
		if (GUILayout.Button (new GUIContent ("Apply Dimension Changes", "Set the dimensions of the level to these values."))) {
			ExpandArray ();
		}

		GUILayout.Label (new GUIContent ("Level Grid", "Checked is floor, unchecked is wall."), EditorStyles.boldLabel);

		ChangeGridWidthAndHeight ();
		if (GUILayout.Button (new GUIContent ("Build / Update Level", "Please note that this will destroy the existing map. This will create a game controller if you don't have one, then place all the tiles according to the diagram."))) {
			BuildLevel ();
		}
	}

	private static void ExpandArray () {
		length = lengthDisplay;
		width = widthDisplay;

		bool[,] newArray = new bool[length, width];

		// array defaults
		for (int j = 0; j < newArray.GetLength (1); j++) {
			for (int i = 0; i < newArray.GetLength (0); i++) {
				newArray [i, j] = expandedFloorDefault;
			}
		}

		// copies old array, makes sure it is at least as big as the new array
		for (int j = 0; j < Mathf.Min (fieldsArray.GetLength (1), width); j++) {
			for (int i = 0; i < Mathf.Min (fieldsArray.GetLength (0), length); i++) {
				newArray [i, j] = fieldsArray [i, j];
			}
		}

		fieldsArray = newArray;
	}

	private static void ChangeGridWidthAndHeight () {
		for (int j = 0; j < width; j++) {
			EditorGUILayout.BeginHorizontal ();
			for (int i = 0; i < length; i++) {
				fieldsArray [i, j] = EditorGUILayout.Toggle (fieldsArray [i, j], GUILayout.ExpandWidth (false), GUILayout.Width (15f));
			}
			EditorGUILayout.EndHorizontal ();
		}
	}

	private static void BuildLevel () {
		GameObject.FindGameObjectWithTag ("GameController");
		GameObject found = GameObject.FindGameObjectWithTag ("GameController");
		if (found == null) {
			GameObject g = GameObject.Instantiate (Resources.Load ("GameController")) as GameObject;
			g.name = "GameController";
		}

		found = GameObject.FindGameObjectWithTag ("MapTilesParent");
		if (found != null) {
			DestroyImmediate (found);
		}

		mapTilesParent = new GameObject ("MapTilesParent");
		mapTilesParent.tag = "MapTilesParent";

		for (int j = width - 1; j >= 0; j--) {
			for (int i = 0; i < length; i++) {
				if (fieldsArray [i, j]) {
					InstantiateFloor (new Vector3 (i, 0, width - j - 1));
				}
				else {
					InstantiateWall (new Vector3 (i, 0, width - j - 1));
				}
			}
		}
	}

	private static void InstantiateFloor (Vector3 pos) {
		GameObject g = GameObject.Instantiate (Resources.Load ("FloorTile"), pos, Quaternion.identity) as GameObject;
		g.transform.SetParent (mapTilesParent.transform);
	}
	private static void InstantiateWall (Vector3 pos) {
		GameObject g = GameObject.Instantiate (Resources.Load ("WallTile"), pos, Quaternion.identity) as GameObject;
		g.transform.SetParent (mapTilesParent.transform);
	}
}