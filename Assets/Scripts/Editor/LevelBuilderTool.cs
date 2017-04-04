using UnityEditor;
using UnityEngine;

using System.Collections.Generic;

// put this shit in a namespace please

public class LevelBuilderTool : EditorWindow {

	/// <summary>
	/// All things that will go into instantiating a dog.
	/// </summary>
	private class DogBlueprint {
		public string name;
		public Compass.Direction direction;
		public Point2D point;

		public DogBlueprint (string n, Compass.Direction d, int x, int y) {
			name = n;
			direction = d;
			point = new Point2D (x, y);
		}
	}

	public GameObject floorTile;
	public GameObject wallTile;
	public GameObject gameController;
	public GameObject dogPrefab;

	// Add menu item to the upper bar
	[MenuItem ("Stealth/Level Builder")]
	public static void ShowWindow () {
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow (typeof(LevelBuilderTool));
	}

	private bool[,] fieldsArray = new bool[0, 0];
	private int length = 10;
	private int width = 10;
	private int lengthDisplay = 10;
	private int widthDisplay = 10;

	private bool instructionsFoldout = false;

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

	private bool startup = true;

	/// <summary>
	/// Builds the ui.
	/// </summary>
	void OnGUI () {
		if (startup) {
			ExpandArray ();
			startup = false;
			dogList = new List<DogBlueprint> ();
		}
			
		instructionsFoldout = EditorGUILayout.Foldout (instructionsFoldout, "INSTRUCTIONS", true);
		if (instructionsFoldout) {
			EditorGUILayout.LabelField (" * If you're making a level from scratch, delete the main camera placed in the scene by default.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.LabelField (" * This does not support pathing nodes right now. You'll have to do those manually, but that's easy to do.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.LabelField (" * After that, just CTRL+click on every tile that you want a pathing node on.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.LabelField (" * First, disable the canvas so it doesn't get in the way.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.LabelField (" * Make sure you're getting the root note of the tile and not the mesh or visualizer or something. Its name should be FloorTile.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.LabelField (" * Then link all of the pathing nodes. The connections don't have to be two-way. That's filled out automatically.", EditorStyles.wordWrappedLabel);
		}
		extraSettingsFoldout = EditorGUILayout.Foldout (extraSettingsFoldout, "Additional Settings", true);
		if (extraSettingsFoldout) {
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

		tileImportExportFoldout = EditorGUILayout.Foldout (tileImportExportFoldout, "Tile Import/Export to text", true);
		if (tileImportExportFoldout) {
			EditorStyles.textField.wordWrap = false;
			tileMetaText = EditorGUILayout.TextArea (tileMetaText);

			if (GUILayout.Button (new GUIContent ("Import", "Reads text area, fills out editor data."))) {
				ImportLevelTiles ();
			}
			if (GUILayout.Button (new GUIContent ("Export", "Converts editor data into text."))) {
				ExportLevelTiles ();
			}
		}

		dogImportExportFoldout = EditorGUILayout.Foldout (dogImportExportFoldout, "Dog Import/Export to text", true);
		if (dogImportExportFoldout) {
			EditorStyles.textField.wordWrap = false;
			dogMetaText = EditorGUILayout.TextArea (dogMetaText);

			if (GUILayout.Button (new GUIContent ("Import", "Reads text area, fills out editor data."))) {
				//
			}
			if (GUILayout.Button (new GUIContent ("Export", "Converts editor data into text."))) {
				//
			}
		}

		DogBlueprint deletThis = null;
		dogListFoldout = EditorGUILayout.Foldout (dogListFoldout, new GUIContent ("Dogs", "Must build level to update dogs."), true);
		if (dogListFoldout) {
			foreach (DogBlueprint dbp in dogList) {
				EditorGUILayout.BeginHorizontal ();
				dbp.name = EditorGUILayout.TextField (dbp.name);
				GUILayout.Label (new GUIContent ("Coordinates", "(X,Z) coordinates of the dog."));
				dbp.point.x = EditorGUILayout.IntField (dbp.point.x);
				dbp.point.y = EditorGUILayout.IntField (dbp.point.y);
				if (GUILayout.Button (new GUIContent ("Delete", "Remove this dog."))) {
					deletThis = dbp;
				}
				EditorGUILayout.EndHorizontal ();
			}
			if (GUILayout.Button (new GUIContent ("Add dog", "Add a new dog"))) {
				dogList.Add (new DogBlueprint ("New Dog", Compass.Direction.North, 0, 0));
			}
		}
		if (deletThis != null) {
			dogList.Remove (deletThis);
			deletThis = null;
		}


	}

	/// <summary>
	/// Expands the array.
	/// </summary>
	private void ExpandArray () {
		length = lengthDisplay;
		width = widthDisplay;

		bool[,] newArray = new bool[width, length];

		// array defaults
		for (int j = 0; j < newArray.GetLength (1); j++) {
			for (int i = 0; i < newArray.GetLength (0); i++) {
				newArray [i, j] = expandedFloorDefault;
			}
		}

		// copies old array, makes sure it is at least as big as the new array
		for (int j = 0; j < Mathf.Min (fieldsArray.GetLength (1), length); j++) {
			for (int i = 0; i < Mathf.Min (fieldsArray.GetLength (0), width); i++) {
				newArray [i, j] = fieldsArray [i, j];
			}
		}

		fieldsArray = newArray;
	}

	/// <summary>
	/// Updates the physical representation of the array.
	/// </summary>
	private void ChangeGridWidthAndHeight () {
		//using (new EditorGUI.DisabledScope (true)) {
		for (int j = 0; j < length; j++) {
			EditorGUILayout.BeginHorizontal ();
			for (int i = 0; i < width; i++) {
				fieldsArray [i, j] = EditorGUILayout.Toggle (fieldsArray [i, j], GUILayout.ExpandWidth (false), GUILayout.Width (15f));
			}
			EditorGUILayout.EndHorizontal ();
		}
		//}
	}

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
				if (fieldsArray [i, j]) {
					InstantiateFloor (new Vector3 (i, 0, length - j - 1));
				}
				else {
					InstantiateWall (new Vector3 (i, 0, length - j - 1));
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

		HashSet<Point2D> points = new HashSet<Point2D> ();
		foreach (DogBlueprint dbp in dogList) {
			if (!points.Contains (dbp.point)) {
				InstantiateDog (dbp);
			}
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
	private void InstantiateDog (DogBlueprint dbp) {
		GameObject g = PrefabUtility.InstantiatePrefab (dogPrefab) as GameObject;
		g.transform.position = new Vector3 (dbp.point.x, 0.5f, dbp.point.y);
		g.transform.rotation = Compass.DirectionToRotation (dbp.direction);
		g.transform.SetParent (dogParent.transform);
		g.GetComponent<Dog> ().orientation = dbp.direction;
	}
		
	private void ExportLevelTiles () {
		tileMetaText = "";
		for (int j = 0; j < length; j++) {
			for (int i = 0; i < width; i++) {
				if (fieldsArray [i, j]) {
					tileMetaText += "1";	//floor
				}
				else {
					tileMetaText += "0";	//wall
				}
			}
			tileMetaText += "\n";
		}
		/*foreach (DogBlueprint dbp in dogList) {
			metaText += dbp.name + "\n";
			metaText += dbp.point.x + "," + dbp.point.y + "\n";
			metaText += dbp.direction.ToStringCustom () + "\n";
		}*/
		tileMetaText = tileMetaText.Substring (0, tileMetaText.Length - 1);
	}

	private void ImportLevelTiles () {
		List<string> lines = new List<string> ();
		char[] metaChars = tileMetaText.ToCharArray ();

		string curLine = "";

		for (int i = 0; i < metaChars.Length; i++) {
			if (metaChars [i] == '\n') {
				lines.Add (curLine);
				curLine = "";
			}
			else {
				curLine += metaChars [i];
			}
		}
		lines.Add (curLine);

		lengthDisplay = lines.Count;
		widthDisplay = MaxLineLength (lines);
		ExpandArray ();
		ChangeGridWidthAndHeight ();

		//then change the actual data

		for (int j = 0; j < length; j++) {
			for (int i = 0; i < width; i++) {
				if (CharAt (lines [j], i) == '1') {
					fieldsArray [i, j] = true;
				}
				else if (CharAt (lines [j], i) == '0') {
					fieldsArray [i, j] = false;
				}
			}
		}
	}

	private int MaxLineLength (List<string> aList) {
		int currentMax = 0;
		foreach (string str in aList) {
			if (str.Length > currentMax) {
				currentMax = str.Length;
			}
		}
		return currentMax;
	}

	private char CharAt (string str, int index) {
		return str.Substring (index, 1).ToCharArray () [0];
	}
}