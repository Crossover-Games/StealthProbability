using UnityEditor;
using UnityEngine;

using System.Collections.Generic;

namespace LevelBuilder {
	public partial class LevelBuilderTool : EditorWindow {
		private void DrawPathControl (DogBlueprint dbp) {
			for (int j = 0; j < length; j++) {
				EditorGUILayout.BeginHorizontal ();
				for (int i = 0; i < width; i++) {
					Texture buttonImage = wallImage;
					PathNodeState changeTo = PathNodeState.Empty;

					if (dbp.point.x == i && dbp.point.z == j) { // dog's location
						changeTo = PathNodeState.DogOrigin;
						switch (dbp.direction) {
							case Compass.Direction.North:
								buttonImage = dogNorthImage;
								break;
							case Compass.Direction.South:
								buttonImage = dogSouthImage;
								break;
							case Compass.Direction.East:
								buttonImage = dogEastImage;
								break;
							case Compass.Direction.West:
								buttonImage = dogWestImage;
								break;
						}
					}
					else if (!fieldsArray [i, j]) {  // wall
						buttonImage = wallImage;
						changeTo = PathNodeState.Wall;
					}
					else if (dbp.nodeMap [i, j] == PathNodeState.NormalNode) {  // normal node
						buttonImage = normalNodeImage;
						changeTo = PathNodeState.StopNode;
					}
					else if (dbp.nodeMap [i, j] == PathNodeState.StopNode) {  // stop node
						buttonImage = stopNodeImage;
						changeTo = PathNodeState.Empty;
					}
					else {  // empty floor
						buttonImage = floorImage;
						changeTo = PathNodeState.NormalNode;
					}

					if (GUILayout.Button (buttonImage)) {
						dbp.nodeMap [i, j] = changeTo;
						Debug.Log (dbp.name);
					}
					//fieldsArray [i, j] = EditorGUILayout.Toggle (fieldsArray [i, j], GUILayout.ExpandWidth (false), GUILayout.Width (15f));
				}
				EditorGUILayout.EndHorizontal ();
			}
		}
	}
}