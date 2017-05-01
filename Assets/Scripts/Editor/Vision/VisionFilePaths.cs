using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisionPatternEditor {
	public class VisionFilePaths : ScriptableObject {
		[SerializeField] private Object visionEnumScriptSerialized;
		[SerializeField] private Object visionPatternScriptSerialized;

		public static Object visionEnumScript {
			get {
				return ScriptableObject.CreateInstance<VisionFilePaths> ().visionEnumScriptSerialized;
			}
		}
		public static Object visionPatternScript {
			get {
				return ScriptableObject.CreateInstance<VisionFilePaths> ().visionPatternScriptSerialized;
			}
		}
	}
}