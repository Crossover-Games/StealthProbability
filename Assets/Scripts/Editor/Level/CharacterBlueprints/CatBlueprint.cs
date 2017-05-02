using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelBuilderRemake {
	[System.Serializable]
	public class CatBlueprint : CharacterBlueprint {

		public int energy = 4;

		/// <summary>
		/// Construct a new CatBlueprint.
		/// </summary>
		public static CatBlueprint CreateCatBlueprint (string name, Compass.Direction orientation, Point2D location, int energy) {
			CatBlueprint cbp = ScriptableObject.CreateInstance<CatBlueprint> ();
			cbp.characterName = name;
			cbp.orientation = orientation;
			cbp.location = location;
			cbp.energy = energy;
			return cbp;
		}

		public bool DrawData () {
			EditorGUILayout.BeginHorizontal ();
			characterName = EditorGUILayout.TextField (characterName);
			energy = EditorGUILayout.IntField (Mathf.Clamp (energy, 0, 10));
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
