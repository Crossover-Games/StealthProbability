﻿using UnityEditor;
using UnityEngine;

using System.Collections.Generic;

// put this shit in a partial class please

public class LevelBuilderTool : EditorWindow {

	/// <summary>
	/// All things that will go into instantiating a dog.
	/// </summary>
	private class DogBlueprint {
		public string name;
		public Compass.Direction direction;
		public Point2D point;

		public DogBlueprint (string n, Compass.Direction d, int x, int z) {
			name = n;
			direction = d;
			point = new Point2D (x, z);
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
		EditorWindow.GetWindow (typeof (LevelBuilderTool));
	}

	private bool [,] fieldsArray = new bool [0, 0];
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

	private Vector2 levelScrollPos;
	private Vector2 dogScrollPos;

	private bool startup = true;

	/// <summary>
	/// Builds the ui.
	/// </summary>
	void OnGUI () {
		if (startup) {
			ExpandArray ();
			startup = false;
			dogList = new List<DogBlueprint> ();
			levelScrollPos = Vector2.zero;
			dogScrollPos = Vector2.zero;
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
		widthDisplay = Mathf.Clamp (EditorGUILayout.IntField (new GUIContent ("Width", "X dimension size of the level, in tiles"), widthDisplay), 1, 500);
		lengthDisplay = Mathf.Clamp (EditorGUILayout.IntField (new GUIContent ("Length", "Z dimension size of the level, in tiles"), lengthDisplay), 1, 500);
		if (GUILayout.Button (new GUIContent ("Apply Dimension Changes", "Set the dimensions of the level to these values."))) {
			ExpandArray ();
		}

		GUILayout.Label (new GUIContent ("Level Grid", "Checked is floor, unchecked is wall."), EditorStyles.boldLabel);

		levelScrollPos = EditorGUILayout.BeginScrollView (levelScrollPos);
		// Draws the array
		DrawFieldsArray ();
		EditorGUILayout.EndScrollView ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.Space ();
		if (GUILayout.Button (new GUIContent ("Add Top", "Adds one row on top, shifts everything down."))) {
			lengthDisplay++;
			ExpandArray ();
			for (int i = 0; i < width; i++) {
				for (int j = length - 2; j > 0; j--) {
					fieldsArray [i, j] = fieldsArray [i, j - 1];
				}
				fieldsArray [i, 0] = expandedFloorDefault;
			}
		}
		if (GUILayout.Button (new GUIContent ("Delete Top", "Delete uppermost row, shift everything up."))) {
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < length - 1; j++) {
					fieldsArray [i, j] = fieldsArray [i, j + 1];
				}
			}
			lengthDisplay--;
			ExpandArray ();
		}
		EditorGUILayout.Space ();
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button (new GUIContent ("Add Left", "Adds one column to left, shifts everything right."))) {
			widthDisplay++;
			ExpandArray ();
			for (int j = 0; j < length; j++) {
				for (int i = width - 2; i > 0; i--) {
					fieldsArray [i, j] = fieldsArray [i - 1, j];
				}
				fieldsArray [0, j] = expandedFloorDefault;
			}
		}
		if (GUILayout.Button (new GUIContent ("Delete Left", "Delete leftmost column, shift everything left."))) {
			for (int i = 0; i < width - 1; i++) {
				for (int j = 0; j < length; j++) {
					fieldsArray [i, j] = fieldsArray [i + 1, j];
				}
			}
			widthDisplay--;
			ExpandArray ();
		}
		EditorGUILayout.Space ();
		if (GUILayout.Button (new GUIContent ("Add Right", "Adds one column to the right of everything."))) {
			widthDisplay++;
			ExpandArray ();
		}
		if (GUILayout.Button (new GUIContent ("Delete Right", "Delete rightmost column."))) {
			widthDisplay--;
			ExpandArray ();
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.Space ();
		if (GUILayout.Button (new GUIContent ("Add Bottom", "Adds one row below everything."))) {
			lengthDisplay++;
			ExpandArray ();
		}
		if (GUILayout.Button (new GUIContent ("Delete Bottom", "Delete lowest row."))) {
			lengthDisplay--;
			ExpandArray ();
		}
		EditorGUILayout.Space ();
		EditorGUILayout.EndHorizontal ();


		DogBlueprint deletThis = null;
		dogListFoldout = EditorGUILayout.Foldout (dogListFoldout, new GUIContent ("Dogs", "Must build level to update dogs."), true);
		if (dogListFoldout) {
			dogScrollPos = EditorGUILayout.BeginScrollView (dogScrollPos);
			foreach (DogBlueprint dbp in dogList) {
				EditorGUILayout.BeginHorizontal ();
				dbp.name = EditorGUILayout.TextField (dbp.name);
				GUILayout.Label (new GUIContent ("Coordinates", "(X,Z) coordinates of the dog."));
				dbp.point.x = EditorGUILayout.IntField (dbp.point.x);
				dbp.point.y = EditorGUILayout.IntField (dbp.point.y);
				dbp.direction = (Compass.Direction)EditorGUILayout.EnumPopup (dbp.direction);
				if (GUILayout.Button (new GUIContent ("Delete", "Remove this dog."))) {
					deletThis = dbp;
				}
				EditorGUILayout.EndHorizontal ();
			}
			EditorGUILayout.EndScrollView ();
			if (GUILayout.Button (new GUIContent ("Add dog", "Add a new dog"))) {
				dogList.Add (new DogBlueprint ("New Dog", Compass.Direction.North, 0, 0));
			}
		}
		if (deletThis != null) {
			dogList.Remove (deletThis);
			deletThis = null;
		}

		if (GUILayout.Button (new GUIContent ("Build / Update Level", "Please note that this will destroy the existing map. This will create a game controller if you don't have one, then place all the tiles according to the diagram."))) {
			BuildLevel ();
		}
		if (GUILayout.Button (new GUIContent ("Scan Level", "Scan the existing level for editing."))) {
			ScanLevel ();
		}
		if (GUILayout.Button (new GUIContent ("Create Pathing Nodes", "sketchy af rn tbh 100."))) {
			CreatePathingNodes ();
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
			//dogMetaText = EditorGUILayout.TextArea (dogMetaText);
			dogMetaText = EditorGUILayout.TextArea ("Not implemented yet.");

			if (GUILayout.Button (new GUIContent ("Import", "Reads text area, fills out editor data."))) {
				//
			}
			if (GUILayout.Button (new GUIContent ("Export", "Converts editor data into text."))) {
				//
			}
		}
	}

	/// <summary>
	/// Expands the array.
	/// </summary>
	private void ExpandArray () {
		length = lengthDisplay;
		width = widthDisplay;

		bool [,] newArray = new bool [width, length];

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
	private void DrawFieldsArray () {
		for (int j = 0; j < length; j++) {
			EditorGUILayout.BeginHorizontal ();
			for (int i = 0; i < width; i++) {
				fieldsArray [i, j] = EditorGUILayout.Toggle (fieldsArray [i, j], GUILayout.ExpandWidth (false), GUILayout.Width (15f));
			}
			EditorGUILayout.EndHorizontal ();
		}
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
				Point2D g2w = GridToWorld (i, j);
				Vector3 worldPos = new Vector3 (g2w.x, 0f, g2w.y);
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

		HashSet<Point2D> points = new HashSet<Point2D> ();
		foreach (DogBlueprint dbp in dogList) {
			if (!points.Contains (dbp.point)) {
				InstantiateDog (dbp);
			}
		}
		ExportLevelTiles ();
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
					tileMetaText += "1";    //floor
				}
				else {
					tileMetaText += "0";    //wall
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
		char [] metaChars = tileMetaText.ToCharArray ();

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
		DrawFieldsArray ();

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

	private bool TileRaycastHelper (float x, float z, out RaycastHit hit) {
		return Physics.Raycast (new Vector3 (x, -100f, z), Vector3.up, out hit);
	}
	private bool TileRaycastHelper (Vector3 rayOrigin, out RaycastHit hit) {
		return Physics.Raycast (rayOrigin, Vector3.up, out hit);
	}

	/// <summary>
	/// Build pathing nodes around structures. Uses some sketchy guesswork. Don't rely on this solely.
	/// </summary>
	private void CreatePathingNodes () {
		List<Tile> allTiles = new List<Tile> (FindObjectsOfType<Tile> ());

		foreach (Tile t in allTiles) {
			if (!t.traversable) {
				foreach (Tile neighbor in AllNeighbors (t, true)) {
					if (neighbor.traversable && neighbor.GetComponent<PathingNode> () == null) {
						neighbor.gameObject.AddComponent (typeof (PathingNode));
					}
				}
			}
		}
		LinkPathingNodes ();
	}

	private void LinkPathingNodes () {
		List<PathingNode> allNodes = new List<PathingNode> (FindObjectsOfType<PathingNode> ());
		RaycastHit hit;
		foreach (PathingNode pn in allNodes) {
			int connectionCount = 0;
			if (TileRaycastHelper (pn.transform.position + Vector3.forward + Vector3.down * 100, out hit)) {
				if (hit.collider.gameObject.GetComponent<PathingNode> () != null) {
					pn.northConnection = true;
					connectionCount++;
				}
			}
			if (TileRaycastHelper (pn.transform.position + Vector3.back + Vector3.down * 100, out hit)) {
				if (hit.collider.gameObject.GetComponent<PathingNode> () != null) {
					pn.southConnection = true;
					connectionCount++;
				}
			}
			if (TileRaycastHelper (pn.transform.position + Vector3.left + Vector3.down * 100, out hit)) {
				if (hit.collider.gameObject.GetComponent<PathingNode> () != null) {
					pn.westConnection = true;
					connectionCount++;
				}
			}
			if (TileRaycastHelper (pn.transform.position + Vector3.right + Vector3.down * 100, out hit)) {
				if (hit.collider.gameObject.GetComponent<PathingNode> () != null) {
					pn.eastConnection = true;
					connectionCount++;
				}
			}
			if (connectionCount >= 3) {
				pn.stoppingPoint = true;
			}
		}
		foreach (DogBlueprint dbp in dogList) {
			if (TileRaycastHelper (dbp.point.x, dbp.point.y, out hit)) {
				PathingNode tempNode = hit.collider.GetComponent<PathingNode> ();
				if (tempNode != null) {
					tempNode.stoppingPoint = true;
				}
			}
		}
	}

	/// <summary>
	/// Returns all neighbors.
	/// </summary>
	private List<Tile> AllNeighbors (Tile t, bool includeDiagonals) {
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