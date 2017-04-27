using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelBuilderRemake {
	public class DogBlueprint : CharacterBlueprint {

		[System.Serializable]
		public class FlatArray2DPathing : FlatArray2D<PathNodeState> { }
		public FlatArray2DPathing nodeMap;
		public DogVisionPatternType visionType;

		/// <summary>
		/// Construct a new DogBlueprint.
		/// </summary>
		public static DogBlueprint CreateDogBlueprint (string name, Compass.Direction orientation, Point2D location, DogVisionPatternType visionType, LevelBlueprint lbp) {
			DogBlueprint dbp = ScriptableObject.CreateInstance<DogBlueprint> () as DogBlueprint;
			dbp.characterName = name;
			dbp.orientation = orientation;
			dbp.location = location;
			dbp.nodeMap.Set2DShallow (new PathNodeState [lbp.tiles.GetLength (0), lbp.tiles.GetLength (1)]);
			dbp.visionType = visionType;
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
