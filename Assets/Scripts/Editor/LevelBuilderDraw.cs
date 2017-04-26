﻿using UnityEditor;
using UnityEngine;

using System.Collections.Generic;

namespace LevelBuilder {
	public partial class LevelBuilderTool : EditorWindow {
		/// <summary>
		/// Builds the ui.
		/// </summary>
		void OnGUI () {
			if (startup) {
				startup = false;
				dogList = new List<DogBlueprint> ();
				levelScrollPos = Vector2.zero;
				dogScrollPos = Vector2.zero;
				ExpandArray ();
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
				fieldsArray = fieldsArray.RowInsertedAtZero (expandedFloorDefault);
				foreach (DogBlueprint dbp in dogList) {
					dbp.nodeMap = dbp.nodeMap.RowInsertedAtZero (expandedFloorDefault ? PathNodeState.Empty : PathNodeState.Wall);
				}
				lengthDisplay++;
				ExpandArray ();
			}
			if (GUILayout.Button (new GUIContent ("Delete Top", "Delete uppermost row, shift everything up."))) {
				fieldsArray = fieldsArray.RowRemovedAtZero ();
				foreach (DogBlueprint dbp in dogList) {
					dbp.nodeMap = dbp.nodeMap.RowRemovedAtZero ();
				}
				lengthDisplay--;
				ExpandArray ();
			}
			EditorGUILayout.Space ();
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button (new GUIContent ("Add Left", "Adds one column to left, shifts everything right."))) {
				fieldsArray = fieldsArray.ColumnInsertedAtZero (expandedFloorDefault);
				foreach (DogBlueprint dbp in dogList) {
					dbp.nodeMap = dbp.nodeMap.ColumnInsertedAtZero (expandedFloorDefault ? PathNodeState.Empty : PathNodeState.Wall);
				}
				widthDisplay++;
				ExpandArray ();
			}
			if (GUILayout.Button (new GUIContent ("Delete Left", "Delete leftmost column, shift everything left."))) {
				fieldsArray = fieldsArray.ColumnRemovedAtZero ();
				foreach (DogBlueprint dbp in dogList) {
					dbp.nodeMap = dbp.nodeMap.ColumnRemovedAtZero ();
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
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.Space ();
				if (GUILayout.Button (new GUIContent ("Add dog", "Add a new dog"))) {
					dogList.Add (new DogBlueprint ("New Dog", Compass.Direction.North, 0, 0));
					ExpandArray ();
				}
				EditorGUILayout.Space ();
				EditorGUILayout.EndHorizontal ();

				dogScrollPos = EditorGUILayout.BeginScrollView (dogScrollPos);
				foreach (DogBlueprint dbp in dogList) {
					EditorGUILayout.BeginHorizontal ();
					dbp.name = EditorGUILayout.TextField (dbp.name);
					GUILayout.Label (new GUIContent ("Coordinates", "(X,Z) coordinates of the dog."));
					dbp.point.x = EditorGUILayout.IntField (dbp.point.x);
					dbp.point.z = EditorGUILayout.IntField (dbp.point.z);
					dbp.direction = (Compass.Direction)EditorGUILayout.EnumPopup (dbp.direction);
					if (GUILayout.Button (new GUIContent ("Delete", "Remove this dog."))) {
						deletThis = dbp;
					}
					EditorGUILayout.EndHorizontal ();
					dbp.pathFoldout = EditorGUILayout.Foldout (dbp.pathFoldout, new GUIContent ("Path Map", "Build the path of this dog only."), true);
					if (dbp.pathFoldout) {
						DrawPathControl (dbp);
					}
				}
				EditorGUILayout.EndScrollView ();
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
	}
}
