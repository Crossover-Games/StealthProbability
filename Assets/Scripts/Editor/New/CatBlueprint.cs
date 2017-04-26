using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelBuilderRemake {
	public class CatBlueprint : CharacterBlueprint {
		/// <summary>
		/// Construct a new CatBlueprint.
		/// </summary>
		public static CatBlueprint CreateCatBlueprint (string name, Compass.Direction orientation, Point2D location) {
			CatBlueprint cbp = ScriptableObject.CreateInstance<CatBlueprint> () as CatBlueprint;
			cbp.characterName = name;
			cbp.orientation = orientation;
			cbp.location = location;
			return cbp;
		}

		public override bool DrawData () {
			throw new NotImplementedException ();
		}
	}
}
