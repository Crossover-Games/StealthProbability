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

					if (dbp.point.x == i && dbp.point.y == j) { // dog's location
						buttonImage = dogOriginImage;
						changeTo = PathNodeState.DogOrigin;
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