using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelBuilderRemake {
	public class DogBlueprint : CharacterBlueprint {

		public PathNodeState [,] nodeMap;

		/// <summary>
		/// Construct a new DogBlueprint.
		/// </summary>
		public static DogBlueprint CreateDogBlueprint (string name, Compass.Direction orientation, Point2D location) {
			DogBlueprint dbp = ScriptableObject.CreateInstance<DogBlueprint> () as DogBlueprint;
			dbp.characterName = name;
			dbp.orientation = orientation;
			dbp.location = location;
			return dbp;
		}

		/// <summary>
		/// Draws the fields of this dog in the editor. True if this is to be deleted.
		/// </summary>
		public override bool DrawData () {
			throw new NotImplementedException ();
		}
	}
}
