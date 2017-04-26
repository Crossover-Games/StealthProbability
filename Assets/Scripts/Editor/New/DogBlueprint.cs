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

		public override bool DrawData () {
			throw new NotImplementedException ();
		}
	}
}
