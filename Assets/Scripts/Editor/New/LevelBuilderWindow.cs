using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelBuilderRemake {
	public class LevelBuilderWindow : EditorWindow {
		// Add menu item to the upper bar
		[MenuItem ("Stealth/Level Builder Remake")]
		public static void ShowWindow () {
			//Show existing window instance. If one doesn't exist, make one.
			EditorWindow.GetWindow (typeof (LevelBuilderWindow));
		}

		void OnGUI () {
			GUILayout.Label ("not fkn implemented", EditorStyles.boldLabel);
		}
	}
}