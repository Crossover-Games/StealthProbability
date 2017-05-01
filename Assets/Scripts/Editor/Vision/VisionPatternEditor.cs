using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
namespace VisionPatternEditor {
	/// <summary>
	/// Vision Pattern Editor
	/// </summary>
	public class VisionPatternEditorWindow : EditorWindow {

		[SerializeField] private ProbabilityGrid currentPattern = ProbabilityGrid.CreateEmptyGrid (1);
		[SerializeField] private string patternName = "NewVisionPattern";
		[SerializeField] private int radiusDisplay = 0;

		[SerializeField] private bool folderFoldout = false;
		[SerializeField] private bool instructionsFoldout = false;

		[MenuItem ("Stealth/Vision Pattern Editor")]
		public static void ShowWindow () {
			EditorWindow.GetWindow<VisionPatternEditorWindow> ();
		}

		private Color DangerToColor (float danger) {
			if (danger > 0.98f) {
				return Color.gray;
			}
			else if (danger > 0.66f) {
				return Color.red;
			}
			else if (danger > 0.33f) {
				return Color.yellow;
			}
			else if (danger > 0.01f) {
				return Color.green;
			}
			else {
				return Color.white;
			}
		}

		public void OnGUI () {
			GUI.backgroundColor = Color.white;
			patternName = EditorGUILayout.TextField (patternName);
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button (new GUIContent ("Load", "Load the pattern with this name."))) {
				currentPattern = ProbabilityGrid.LoadFromResources (patternName);
				Debug.Log (currentPattern.Length);
				radiusDisplay = (currentPattern.GetLength (0) - 1) / 2;
				currentPattern [radiusDisplay, radiusDisplay] = 1f;
			}
			if (GUILayout.Button (new GUIContent ("Save", "Save pattern to Assets/Resources/VisionPatterns."))) {
				File.WriteAllText ("Assets/Resources/VisionPatterns/" + patternName + ".txt", JsonUtility.ToJson (currentPattern));
				AssetDatabase.SaveAssets ();
				AssetDatabase.Refresh ();
			}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			radiusDisplay = EditorGUILayout.IntField (new GUIContent ("Radius", "Radius of dog vision"), radiusDisplay);
			if (GUILayout.Button (new GUIContent ("Apply size change", "."))) {
				currentPattern.Set2DShallow (new float [radiusDisplay * 2 + 1, radiusDisplay * 2 + 1]);
				currentPattern [radiusDisplay, radiusDisplay] = 1f;
			}
			EditorGUILayout.EndHorizontal ();

			for (int i = 0; i < currentPattern.GetLength (0); i++) {
				EditorGUILayout.BeginHorizontal ();
				for (int j = 0; j < currentPattern.GetLength (1); j++) {
					GUI.backgroundColor = DangerToColor (currentPattern [i, j]);
					currentPattern [i, j] = EditorGUILayout.FloatField (currentPattern [i, j]);
				}
				EditorGUILayout.EndHorizontal ();
			}
			instructionsFoldout = EditorGUILayout.Foldout (instructionsFoldout, new GUIContent ("Instructions", "what ye need to know."), true);
			if (instructionsFoldout) {
				EditorStyles.label.wordWrap = true;
				EditorGUILayout.LabelField ("First, make and save your vision pattern. Then create an entry in the enum in DogVisionPatternType for it. Then make a new switch statement in VisionPattern.VisionPatternFromType that maps your file name (without extension) to the enum entry you made for it.");
			}
			folderFoldout = EditorGUILayout.Foldout (folderFoldout, new GUIContent ("Files", "Important files related to vision."), true);
			if (folderFoldout) {
				if (GUILayout.Button (new GUIContent ("Navigate to DogVisionPatternType.cs", "Enum entry required for each dog pattern."))) {
					EditorGUIUtility.PingObject (VisionFilePaths.visionEnumScript);
				}
				if (GUILayout.Button (new GUIContent ("Navigate to VisionPattern.cs", "Switch required for DogVisionPatternType."))) {
					EditorGUIUtility.PingObject (VisionFilePaths.visionPatternScript);
				}
				if (GUILayout.Button (new GUIContent ("Pattern File Folder", "Folder containing all vision pattern files."))) {
					File.WriteAllText ("Assets/Resources/VisionPatterns/dummy.txt", JsonUtility.ToJson (currentPattern));
					AssetDatabase.SaveAssets ();
					EditorGUIUtility.PingObject (Resources.Load ("VisionPatterns/dummy"));
				}
			}
		}
	}
}