using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelBuilderRemake {
	public class DogBlueprintPathDraw : ScriptableObject {
		public Texture floorImage;
		public Texture wallImage;
		public Texture normalNodeImage;
		public Texture stopNodeImage;
		public Texture dogNorthImage;
		public Texture dogSouthImage;
		public Texture dogEastImage;
		public Texture dogWestImage;

		/// <summary>
		/// Draw the path editor of a dog.
		/// </summary>
		public static void DrawPathEditor (DogBlueprint dbp, LevelBlueprint levelBP) {
			DogBlueprintPathDraw images = ScriptableObject.CreateInstance<DogBlueprintPathDraw> ();
			for (int j = levelBP.tiles.GetLength (1) - 1; j >= 0; j--) {
				EditorGUILayout.BeginHorizontal ();
				for (int i = 0; i < levelBP.tiles.GetLength (0); i++) {
					Texture buttonImage = images.wallImage;
					PathNodeState changeTo = PathNodeState.Empty;

					switch (dbp.nodeMap [i, j]) {
						case PathNodeState.NormalNode:
							GUI.color = Color.blue;
							changeTo = PathNodeState.StopNode;
							break;
						case PathNodeState.StopNode:
							GUI.color = Color.red;
							changeTo = PathNodeState.Empty;
							break;
						case PathNodeState.Empty:
							GUI.color = Color.white;
							changeTo = PathNodeState.NormalNode;
							break;
					}

					if (new Point2D (i, j) == dbp.location) {
						GUI.color = Color.red;
						switch (dbp.orientation) {
							case Compass.Direction.North:
								buttonImage = images.dogNorthImage;
								break;
							case Compass.Direction.South:
								buttonImage = images.dogSouthImage;
								break;
							case Compass.Direction.East:
								buttonImage = images.dogEastImage;
								break;
							case Compass.Direction.West:
								buttonImage = images.dogWestImage;
								break;
						}
					}
					else if (!levelBP.tiles [i, j]) {
						buttonImage = images.wallImage;
					}
					else {
						buttonImage = images.floorImage;
					}

					if (GUILayout.Button (buttonImage)) {
						dbp.nodeMap [i, j] = changeTo;
						Debug.Log (dbp.characterName);
					}
					GUI.color = Color.white;
				}
				EditorGUILayout.EndHorizontal ();
			}
		}
	}
}