using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

/// <summary>
/// 
/// </summary>
public class VisionPatternEditor : EditorWindow {

	[SerializeField] private ProbabilityGrid currentPattern = ProbabilityGrid.CreateEmptyGrid (1);
	[SerializeField] private string patternName = "NewVisionPattern";
	[SerializeField] private int radiusDisplay = 0;


	[MenuItem ("Stealth/Vision Pattern Editor")]
	public static void ShowWindow () {
		EditorWindow.GetWindow<VisionPatternEditor> ();
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
			TextAsset ta = Resources.Load<TextAsset> ("VisionPatterns/" + patternName);
			Debug.Log (ta.text);
			currentPattern = JsonUtility.FromJson<ProbabilityGrid> (ta.text);
			Debug.Log (currentPattern.Length);
			radiusDisplay = currentPattern.GetLength (0);
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
	}
}