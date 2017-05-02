using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelBuilderRemake {
	[System.Serializable]
	public class DogBlueprint : CharacterBlueprint {

		[System.Serializable]
		public class FlatArray2DPathing : FlatArray2D<PathNodeState> { }
		public FlatArray2DPathing nodeMap;
		public DogVisionPatternType visionType;
		private bool pathFoldout = false;

		/// <summary>
		/// Construct a new DogBlueprint.
		/// </summary>
		public static DogBlueprint CreateDogBlueprint (string name, Compass.Direction orientation, Point2D location, DogVisionPatternType visionType, LevelBlueprint lbp) {
			DogBlueprint dbp = ScriptableObject.CreateInstance<DogBlueprint> ();
			dbp.characterName = name;
			dbp.orientation = orientation;
			dbp.location = location;
			dbp.nodeMap = new FlatArray2DPathing ();
			dbp.nodeMap.Set2DShallow (new PathNodeState [lbp.tiles.GetLength (0), lbp.tiles.GetLength (1)]);
			dbp.visionType = visionType;
			return dbp;
		}

		/// <summary>
		/// Draws the fields of this dog in the editor. True if this is to be deleted.
		/// </summary>
		public bool DrawData (LevelBlueprint lbp) {
			EditorGUILayout.BeginHorizontal ();
			characterName = EditorGUILayout.TextField (characterName);
			EditorGUILayout.LabelField ("Coordinates");
			Point2D newPoint = new Point2D (location.x, location.z);
			newPoint.x = EditorGUILayout.IntField (newPoint.x);
			newPoint.z = EditorGUILayout.IntField (newPoint.z);
			location = newPoint;
			orientation = (Compass.Direction)EditorGUILayout.EnumPopup (orientation);
			visionType = (DogVisionPatternType)EditorGUILayout.EnumPopup (visionType);
			bool deletThis = GUILayout.Button (new GUIContent ("Delete", "Delete this dog."));
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.Space ();
			pathFoldout = EditorGUILayout.Foldout (pathFoldout, new GUIContent ("Path Map", "Draw the path of this dog only."), true);
			EditorGUILayout.EndHorizontal ();
			if (pathFoldout) {
				DogBlueprintPathDraw.DrawPathEditor (this, lbp);
			}
			return deletThis;
		}


	}
}
