using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelBuilderRemake {
	public class LevelBlueprint {
		[System.Serializable]
		public class FlatArray2DBool : FlatArray2D<bool> { }
		public FlatArray2DBool tiles { get; set; }
		public List<Point2D> victoryTiles { get; set; }
		public List<DogBlueprint> dogs { get; set; }
		public List<CatBlueprint> cats { get; set; }

		/// <summary>
		/// Draws the editor window for tiles. True is floor, false is wall.
		/// </summary>
		public void DrawTilesEditor () {
			for (int j = tiles.GetLength (1) - 1; j >= 0; j++) {
				EditorGUILayout.BeginHorizontal ();
				for (int i = 0; i < tiles.GetLength (0); i++) {
					tiles [i, j] = EditorGUILayout.Toggle (tiles [i, j], GUILayout.ExpandWidth (false), GUILayout.Width (15f));
				}
				EditorGUILayout.EndHorizontal ();
			}
		}


		int widthDisplay = 0;
		int lengthDisplay = 0;
		/// <summary>
		/// Draws the controls for changing map dimensions directly.
		/// </summary>
		public void DrawMapDimensionsControlDirect () {
			widthDisplay = Mathf.Clamp (EditorGUILayout.IntField (new GUIContent ("Width", "X dimension size of the level, in tiles"), widthDisplay), 1, 500);
			lengthDisplay = Mathf.Clamp (EditorGUILayout.IntField (new GUIContent ("Length", "Z dimension size of the level, in tiles"), lengthDisplay), 1, 500);
			if (GUILayout.Button (new GUIContent ("Apply Dimension Changes", "Set the dimensions of the level to these values."))) {
				tiles.SetDimensions (widthDisplay, lengthDisplay, true);
				foreach (DogBlueprint dbp in dogs) {
					dbp.nodeMap.SetDimensions (widthDisplay, lengthDisplay, PathNodeState.Empty);
				}
			}
		}

		/// <summary>
		/// Draws the editor for victory tiles.
		/// </summary>
		public void DrawVictoryTilesEditor () {
			int deleteIndex = -1;
			for (int c = 0; c < victoryTiles.Count; c++) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label (new GUIContent ("Coordinates", "(X,Z) coordinates of the objective."));
				Point2D point = new Point2D ();
				point.x = EditorGUILayout.IntField (victoryTiles [c].x);
				point.z = EditorGUILayout.IntField (victoryTiles [c].z);
				victoryTiles [c] = point;
				if (GUILayout.Button (new GUIContent ("Delete", "Remove this objective."))) {
					deleteIndex = c;
				}
				EditorGUILayout.EndHorizontal ();
			}
			if (deleteIndex >= 0) {
				victoryTiles.RemoveAt (deleteIndex);
			}
		}
		public void DrawDogsEditor () {
			DrawCharacterEditorHelper (dogs);
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.Space ();
			if (GUILayout.Button (new GUIContent ("Add dog", "Add a new dog."))) {
				dogs.Add (DogBlueprint.CreateDogBlueprint ("dog", Compass.Direction.North, new Point2D (0, 0), DogVisionPatternType.Hound, this));
			}
			EditorGUILayout.Space ();
			EditorGUILayout.EndHorizontal ();
		}
		public void DrawCatsEditor () {
			DrawCharacterEditorHelper (cats);
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.Space ();
			if (GUILayout.Button (new GUIContent ("Add cat", "Add a new cat."))) {
				cats.Add (CatBlueprint.CreateCatBlueprint ("cat", Compass.Direction.North, new Point2D (0, 0)));
			}
			EditorGUILayout.Space ();
			EditorGUILayout.EndHorizontal ();
		}

		private void DrawCharacterEditorHelper<T> (List<T> characters)
		where T : CharacterBlueprint {
			T deleteThis = null;
			foreach (T chara in characters) {
				if (chara.DrawData ()) {
					deleteThis = chara;
				}
			}
			if (deleteThis != null) {
				characters.Remove (deleteThis);
			}
		}
	}
}