using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelBuilderRemake {
	public class LevelBlueprint {
		public bool [,] tiles { get; set; }
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
		public void DrawVictoryTilesEditor () {
			foreach (Point2D point in victoryTiles) {
				//draw data, delete option
			}
		}
		public void DrawDogsEditor () {
			DrawCharacterEditorHelper (dogs);
		}
		public void DrawCatsEditor () {
			DrawCharacterEditorHelper (cats);
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