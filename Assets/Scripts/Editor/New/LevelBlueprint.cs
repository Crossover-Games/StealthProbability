using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelBuilderRemake {
	public class LevelBlueprint {
		public bool [,] tiles { get; set; }
		public List<Point2D> victoryTiles { get; set; }
		public List<DogBlueprint> dogs { get; set; }
		public List<CatBlueprint> cats { get; set; }

		public void DrawTilesEditor () { }
		public void DrawVictoryTilesEditor () { }
		public void DrawDogsEditor () { }
		public void DrawCatsEditor () { }
	}
}