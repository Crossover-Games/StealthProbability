using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelBuilderRemake {
	/// <summary>
	/// Extension methods to let a level blueprint draw itself.
	/// </summary>
	public static class LevelBlueprintDraw {
		/// <summary>
		/// Draws the editor window for tiles. True is floor, false is wall.
		/// </summary>
		public static void DrawTilesEditor (this LevelBlueprint levelBP) {
			GUILayout.Label (new GUIContent ("Level Tiles", "True is floor, false is wall. Overriden by victory tiles."), EditorStyles.boldLabel);
			for (int j = levelBP.tiles.GetLength (1) - 1; j >= 0; j--) {
				EditorGUILayout.BeginHorizontal ();
				for (int i = 0; i < levelBP.tiles.GetLength (0); i++) {
					Point2D point = new Point2D (i, j);
					if (levelBP.dogs.Exists ((DogBlueprint dbp) => dbp.location == point)) {
						GUI.color = Color.red;
					}
					else if (levelBP.cats.Exists ((CatBlueprint cbp) => cbp.location == point)) {
						GUI.color = new Color (0.5f, 0f, 1f);
					}
					EditorGUI.BeginDisabledGroup (levelBP.victoryTiles.Contains (point));
					levelBP.tiles [i, j] = EditorGUILayout.Toggle (levelBP.tiles [i, j], GUILayout.ExpandWidth (false), GUILayout.Width (15f));
					EditorGUI.EndDisabledGroup ();
					GUI.color = Color.white;
				}
				EditorGUILayout.EndHorizontal ();
			}
			GUI.color = Color.white;
		}

		/// <summary>
		/// Draws the controls for changing map dimensions directly.
		/// </summary>
		public static void DrawMapDimensionsDirectControl (this LevelBlueprint levelBP) {
			if (levelBP.widthDisplay != levelBP.tiles.GetLength (0)) {
				GUI.color = Color.cyan;
			}
			levelBP.widthDisplay = Mathf.Clamp (EditorGUILayout.IntField (new GUIContent ("Width", "X dimension size of the level, in tiles"), levelBP.widthDisplay), 1, 500);

			if (levelBP.lengthDisplay != levelBP.tiles.GetLength (1)) {
				GUI.color = Color.cyan;
			}
			else {
				GUI.color = Color.white;
			}
			levelBP.lengthDisplay = Mathf.Clamp (EditorGUILayout.IntField (new GUIContent ("Length", "Z dimension size of the level, in tiles"), levelBP.lengthDisplay), 1, 500);
			GUI.color = Color.white;
			if (GUILayout.Button (new GUIContent ("Apply Dimension Changes", "Set the dimensions of the level to these values."))) {
				levelBP.tiles.SetDimensions (levelBP.widthDisplay, levelBP.lengthDisplay, true);
				foreach (DogBlueprint dbp in levelBP.dogs) {
					dbp.nodeMap.SetDimensions (levelBP.widthDisplay, levelBP.lengthDisplay, PathNodeState.Empty);
				}
			}
		}

		/// <summary>
		/// Draws a set of buttons for increasing / decreasing the borders of the level by one.
		/// </summary>
		public static void DrawMapDimensionsButtons (this LevelBlueprint levelBP) {
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.Space ();
			if (GUILayout.Button (new GUIContent ("Add Top", "Adds one row on top, shifts everything down."))) {
				levelBP.tiles.Set2DShallow (levelBP.tiles.Get2DShallow ().ChangedDimensions (levelBP.tiles.GetLength (0), levelBP.tiles.GetLength (1) + 1, true));
				foreach (DogBlueprint dbp in levelBP.dogs) {
					dbp.nodeMap.Set2DShallow (dbp.nodeMap.Get2DShallow ().ChangedDimensions (dbp.nodeMap.GetLength (0), dbp.nodeMap.GetLength (1) + 1, PathNodeState.Empty));
				}
				levelBP.RefreshDimensionDisplay ();
			}
			if (GUILayout.Button (new GUIContent ("Delete Top", "Delete uppermost row, shift everything up."))) {
				levelBP.tiles.Set2DShallow (levelBP.tiles.Get2DShallow ().ChangedDimensions (levelBP.tiles.GetLength (0), levelBP.tiles.GetLength (1) - 1));
				foreach (DogBlueprint dbp in levelBP.dogs) {
					dbp.nodeMap.Set2DShallow (dbp.nodeMap.Get2DShallow ().ChangedDimensions (dbp.nodeMap.GetLength (0), dbp.nodeMap.GetLength (1) - 1));
				}
				levelBP.RefreshDimensionDisplay ();
			}
			EditorGUILayout.Space ();
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button (new GUIContent ("Add Left", "Adds one column to left, shifts everything right."))) {
				levelBP.tiles.Set2DShallow (levelBP.tiles.Get2DShallow ().ColumnInsertedAtZero (true));
				foreach (DogBlueprint dbp in levelBP.dogs) {
					dbp.nodeMap.Set2DShallow (dbp.nodeMap.Get2DShallow ().ColumnInsertedAtZero (PathNodeState.Empty));
				}
				levelBP.RefreshDimensionDisplay ();
			}
			if (GUILayout.Button (new GUIContent ("Delete Left", "Delete leftmost column, shift everything left."))) {
				levelBP.tiles.Set2DShallow (levelBP.tiles.Get2DShallow ().ColumnRemovedAtZero ());
				foreach (DogBlueprint dbp in levelBP.dogs) {
					dbp.nodeMap.Set2DShallow (dbp.nodeMap.Get2DShallow ().ColumnRemovedAtZero ());
				}
				levelBP.RefreshDimensionDisplay ();
			}
			EditorGUILayout.Space ();
			if (GUILayout.Button (new GUIContent ("Add Right", "Adds one column to the right of everything."))) {
				levelBP.tiles.Set2DShallow (levelBP.tiles.Get2DShallow ().ChangedDimensions (levelBP.tiles.GetLength (0) + 1, levelBP.tiles.GetLength (1), true));
				foreach (DogBlueprint dbp in levelBP.dogs) {
					dbp.nodeMap.Set2DShallow (dbp.nodeMap.Get2DShallow ().ChangedDimensions (dbp.nodeMap.GetLength (0) + 1, dbp.nodeMap.GetLength (1), PathNodeState.Empty));
				}
				levelBP.RefreshDimensionDisplay ();
			}
			if (GUILayout.Button (new GUIContent ("Delete Right", "Delete rightmost column."))) {
				levelBP.tiles.Set2DShallow (levelBP.tiles.Get2DShallow ().ChangedDimensions (levelBP.tiles.GetLength (0) - 1, levelBP.tiles.GetLength (1)));
				foreach (DogBlueprint dbp in levelBP.dogs) {
					dbp.nodeMap.Set2DShallow (dbp.nodeMap.Get2DShallow ().ChangedDimensions (dbp.nodeMap.GetLength (0) - 1, dbp.nodeMap.GetLength (1)));
				}
				levelBP.RefreshDimensionDisplay ();
			}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.Space ();
			if (GUILayout.Button (new GUIContent ("Add Bottom", "Adds one row below everything."))) {
				levelBP.tiles.Set2DShallow (levelBP.tiles.Get2DShallow ().RowInsertedAtZero (true));
				foreach (DogBlueprint dbp in levelBP.dogs) {
					dbp.nodeMap.Set2DShallow (dbp.nodeMap.Get2DShallow ().RowInsertedAtZero (PathNodeState.Empty));
				}
				levelBP.RefreshDimensionDisplay ();
			}
			if (GUILayout.Button (new GUIContent ("Delete Bottom", "Delete lowest row."))) {
				levelBP.tiles.Set2DShallow (levelBP.tiles.Get2DShallow ().RowRemovedAtZero ());
				foreach (DogBlueprint dbp in levelBP.dogs) {
					dbp.nodeMap.Set2DShallow (dbp.nodeMap.Get2DShallow ().RowRemovedAtZero ());
				}
				levelBP.RefreshDimensionDisplay ();
			}
			EditorGUILayout.Space ();
			EditorGUILayout.EndHorizontal ();
		}


		/// <summary>
		/// Draws the editor for victory tiles.
		/// </summary>
		public static void DrawVictoryTilesEditor (this LevelBlueprint levelBP) {
			int deleteIndex = -1;
			for (int c = 0; c < levelBP.victoryTiles.Count; c++) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label (new GUIContent ("Coordinates", "(X,Z) coordinates of the objective."));
				Point2D point = new Point2D ();
				point.x = EditorGUILayout.IntField (levelBP.victoryTiles [c].x);
				point.z = EditorGUILayout.IntField (levelBP.victoryTiles [c].z);
				levelBP.victoryTiles [c] = point;
				if (GUILayout.Button (new GUIContent ("Delete", "Remove this objective."))) {
					deleteIndex = c;
				}
				EditorGUILayout.EndHorizontal ();
			}
			if (deleteIndex >= 0) {
				levelBP.victoryTiles.RemoveAt (deleteIndex);
			}
			if (GUILayout.Button (new GUIContent ("Add victory tile", "oi."))) {
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.Space ();
				levelBP.victoryTiles.Add (new Point2D (0, 0));
				EditorGUILayout.Space ();
				EditorGUILayout.EndHorizontal ();
			}
		}

		/// <summary>
		/// All dogs 
		/// </summary>
		public static void DrawDogsEditor (this LevelBlueprint levelBP) {
			DogBlueprint deleteThis = null;
			foreach (DogBlueprint chara in levelBP.dogs) {
				if (chara.DrawData (levelBP)) {
					deleteThis = chara;
				}
			}
			if (deleteThis != null) {
				levelBP.dogs.Remove (deleteThis);
			}
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.Space ();
			if (GUILayout.Button (new GUIContent ("Add dog", "Add a new dog."))) {
				levelBP.dogs.Add (DogBlueprint.CreateDogBlueprint ("dog", Compass.Direction.North, new Point2D (0, 0), DogVisionPatternType.Hound, levelBP));
			}
			EditorGUILayout.Space ();
			EditorGUILayout.EndHorizontal ();
		}

		/// <summary>
		/// all cats
		/// </summary>
		public static void DrawCatsEditor (this LevelBlueprint levelBP) {
			CatBlueprint deleteThis = null;
			foreach (CatBlueprint chara in levelBP.cats) {
				if (chara.DrawData ()) {
					deleteThis = chara;
				}
			}
			if (deleteThis != null) {
				levelBP.cats.Remove (deleteThis);
			}
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.Space ();
			if (GUILayout.Button (new GUIContent ("Add cat", "Add a new cat."))) {
				levelBP.cats.Add (CatBlueprint.CreateCatBlueprint ("cat", Compass.Direction.North, new Point2D (0, 0)));
			}
			EditorGUILayout.Space ();
			EditorGUILayout.EndHorizontal ();
		}
	}
}