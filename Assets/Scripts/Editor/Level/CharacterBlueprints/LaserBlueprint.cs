using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelBuilderRemake {
	[System.Serializable]
	public class LaserBlueprint : CharacterBlueprint {
		public float probability;

		/// <summary>
		/// Construct a new LaserBlueprint.
		/// </summary>
		public static LaserBlueprint CreateLaserBlueprint (string name, Compass.Direction orientation, Point2D location, float probability) {
			LaserBlueprint cbp = ScriptableObject.CreateInstance<LaserBlueprint> ();
			cbp.characterName = name;
			cbp.orientation = orientation;
			cbp.location = location;
			cbp.probability = probability;
			return cbp;
		}

		public bool DrawData () {
			EditorGUILayout.BeginHorizontal ();
			characterName = EditorGUILayout.TextField (characterName);
			probability = EditorGUILayout.Slider (probability, 0f, 1f);
			EditorGUILayout.LabelField ("Coordinates");
			Point2D newPoint = new Point2D (location.x, location.z);
			newPoint.x = EditorGUILayout.IntField (newPoint.x);
			newPoint.z = EditorGUILayout.IntField (newPoint.z);
			location = newPoint;
			orientation = (Compass.Direction)EditorGUILayout.EnumPopup (orientation);
			bool deletThis = GUILayout.Button (new GUIContent ("Delete", "Delete this cat."));
			EditorGUILayout.EndHorizontal ();
			return deletThis;
		}
	}
}