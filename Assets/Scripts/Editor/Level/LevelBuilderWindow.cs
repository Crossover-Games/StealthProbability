using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelBuilderRemake {
	public class LevelBuilderWindow : EditorWindow {
		// Add menu item to the upper bar
		[MenuItem ("Stealth/Level Builder (4-29)")]
		public static void ShowWindow () {
			//Show existing window instance. If one doesn't exist, make one.
			EditorWindow.GetWindow (typeof (LevelBuilderWindow));
		}

		[SerializeField] private LevelBlueprint levelBP = null;
		[SerializeField] private bool expandedGridDefault = true;

		private bool dimensionsFoldout = false;
		private bool catFoldout = false;
		private bool dogFoldout = false;
		private bool laserFoldout = false;
		private bool victoryFolout = false;
		private bool metaFoldout = false;
		private bool buildFoldout = false;
		private string metaText = "";
		private Vector2 dogListScrollPos = Vector2.zero;
		void OnGUI () {
			if (levelBP == null) {
				levelBP = LevelBlueprint.DefaultLevel ();
			}
			levelBP.DrawTilesEditor ();
			dimensionsFoldout = EditorGUILayout.Foldout (dimensionsFoldout, new GUIContent ("Dimensions", "Length and width"), true);
			if (dimensionsFoldout) {
				expandedGridDefault = EditorGUILayout.ToggleLeft (new GUIContent ("Expanded Grid Default", "When new floors are created due to the grid being expanded, are they floors or walls?"), expandedGridDefault);
				levelBP.DrawMapDimensionsDirectControl (expandedGridDefault);
				levelBP.DrawMapDimensionsButtons (expandedGridDefault);
			}
			victoryFolout = EditorGUILayout.Foldout (victoryFolout, new GUIContent ("Victory Tiles", "These will be built instead of actual walls/floors in the locations you specify."), true);
			if (victoryFolout) {
				levelBP.DrawVictoryTilesEditor ();
			}
			catFoldout = EditorGUILayout.Foldout (catFoldout, new GUIContent ("Cats", "generally better than dogs."), true);
			if (catFoldout) {
				levelBP.DrawCatsEditor ();
			}
			laserFoldout = EditorGUILayout.Foldout (laserFoldout, new GUIContent ("Lasers", "don't shine that in someone's eye."), true);
			if (laserFoldout) {
				levelBP.DrawLasersEditor ();
			}
			dogFoldout = EditorGUILayout.Foldout (dogFoldout, new GUIContent ("Dogs", "they bark."), true);
			if (dogFoldout) {
				dogListScrollPos = EditorGUILayout.BeginScrollView (dogListScrollPos);
				levelBP.DrawDogsEditor ();
				EditorGUILayout.EndScrollView ();
			}
			metaFoldout = EditorGUILayout.Foldout (metaFoldout, new GUIContent ("Import/Export", "Import/export to text."), true);
			if (metaFoldout) {
				metaText = EditorGUILayout.TextField (metaText);
				EditorGUILayout.BeginHorizontal ();
				if (GUILayout.Button (new GUIContent ("Import text to level", "Load a level out of whatever is in the text box."))) {
					try {
						levelBP = JsonUtility.FromJson<LevelBlueprint> (metaText);
					}
					catch {
						Debug.Log ("Invalid level text.");
					}
				}
				if (GUILayout.Button (new GUIContent ("Refresh and copy to clipboard", "Converts the current working level BP into text and copies it to the clipboard."))) {
					metaText = JsonUtility.ToJson (levelBP);
					EditorGUIUtility.systemCopyBuffer = metaText;
				}
				EditorGUILayout.EndHorizontal ();
			}
			buildFoldout = EditorGUILayout.Foldout (buildFoldout, new GUIContent ("Build", "Build or scan level."), true);
			if (buildFoldout) {
				EditorGUILayout.BeginHorizontal ();
				if (GUILayout.Button (new GUIContent ("Build Level", "Create a level in the game world out of this."))) {
					LevelAssembler.AssembleLevel (levelBP);
				}
				if (GUILayout.Button (new GUIContent ("Scan Level", "Create a blueprint from the state of the game world."))) {
					levelBP = LevelAssembler.ScanLevel ();
				}
				EditorGUILayout.Space ();
				if (GUILayout.Button (new GUIContent ("Reset Level", "Wipe the level blueprint and create a fresh one."))) {
					levelBP = LevelBlueprint.DefaultLevel ();
				}
				EditorGUILayout.EndHorizontal ();
			}
		}
	}
}