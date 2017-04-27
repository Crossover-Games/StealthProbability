using UnityEditor;
using UnityEngine;

using System.Collections.Generic;

namespace LevelBuilder {
	public partial class LevelBuilderTool : EditorWindow {

		private void ExportLevelTiles () {
			tileMetaText = "";
			for (int j = 0; j < length; j++) {
				for (int i = 0; i < width; i++) {
					if (fieldsArray [i, j]) {
						tileMetaText += "1";    //floor
					}
					else {
						tileMetaText += "0";    //wall
					}
				}
				tileMetaText += "\n";
			}
			/*foreach (DogBlueprint dbp in dogList) {
				metaText += dbp.name + "\n";
				metaText += dbp.point.x + "," + dbp.point.y + "\n";
				metaText += dbp.direction.ToStringCustom () + "\n";
			}*/
			tileMetaText = tileMetaText.Substring (0, tileMetaText.Length - 1);
		}

		private void ImportLevelTiles () {
			List<string> lines = new List<string> ();
			char [] metaChars = tileMetaText.ToCharArray ();

			string curLine = "";

			for (int i = 0; i < metaChars.Length; i++) {
				if (metaChars [i] == '\n') {
					lines.Add (curLine);
					curLine = "";
				}
				else {
					curLine += metaChars [i];
				}
			}
			lines.Add (curLine);

			lengthDisplay = lines.Count;
			widthDisplay = MaxLineLength (lines);
			ExpandArray ();
			DrawFieldsArray ();

			//then change the actual data

			for (int j = 0; j < length; j++) {
				for (int i = 0; i < width; i++) {
					if (CharAt (lines [j], i) == '1') {
						fieldsArray [i, j] = true;
					}
					else if (CharAt (lines [j], i) == '0') {
						fieldsArray [i, j] = false;
					}
				}
			}
		}

		private int MaxLineLength (List<string> aList) {
			int currentMax = 0;
			foreach (string str in aList) {
				if (str.Length > currentMax) {
					currentMax = str.Length;
				}
			}
			return currentMax;
		}

		private char CharAt (string str, int index) {
			return str.Substring (index, 1).ToCharArray () [0];
		}

	}
}