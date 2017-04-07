using UnityEditor;
using UnityEngine;

using System.Collections.Generic;

public class VisionPatternCreator : EditorWindow {

	public GameObject floorTile;
	public GameObject wallTile;
	public GameObject gameController;

	// Add menu item to the upper bar
	[MenuItem ("Stealth/Vision Pattern Creator")]
	public static void ShowWindow () {
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow (typeof(VisionPatternCreator));
	}

	private bool[,] fieldsArray = new bool[0, 0];
	private int length = 10;
	private int width = 10;
	private int lengthDisplay = 10;
	private int widthDisplay = 10;

	private bool extraSettingsFoldout = false;
	private bool expandedFloorDefault = true;
	private int originX = 0;
	private int originZ = 0;

	private bool metaImportExportFoldout = false;
	private string metaText;

	private GameObject mapTilesParent;

	private bool startup = true;

	void OnGUI () {
		if (startup) {
			ExpandArray ();
			startup = false;
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

		metaImportExportFoldout = EditorGUILayout.Foldout (metaImportExportFoldout, "Text Import/Export", true);
		if (metaImportExportFoldout) {
			metaText = EditorGUILayout.TextArea (metaText);

			if (GUILayout.Button (new GUIContent ("Import", "Reads text area, fills out editor data."))) {
				ImportLevel ();
			}
			if (GUILayout.Button (new GUIContent ("Export", "Converts editor data into text."))) {
				ExportLevel ();
			}
		}
	}

	static bool IsMouseOver () {
		return Event.current.type == EventType.Repaint && GUILayoutUtility.GetLastRect ().Contains (Event.current.mousePosition);
	}

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

	// only updates GUI
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
		GameObject.FindGameObjectWithTag ("GameController");
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

	private void ExportLevel () {
		metaText = "";
		for (int j = 0; j < length; j++) {
			for (int i = 0; i < width; i++) {
				if (fieldsArray [i, j]) {
					metaText += "1";	//floor
				}
				else {
					metaText += "0";	//wall
				}
			}
			metaText += "\n";
		}
		metaText = metaText.Substring (0, metaText.Length - 1);
	}

	private void ImportLevel () {
		List<string> lines = new List<string> ();
		char[] metaChars = metaText.ToCharArray ();

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