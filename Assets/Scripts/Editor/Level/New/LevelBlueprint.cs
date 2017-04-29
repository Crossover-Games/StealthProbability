using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelBuilderRemake {
	/// <summary>
	/// All aspects of a level before construction.
	/// </summary>
	[System.Serializable]
	public class LevelBlueprint {
		[System.Serializable]
		public class FlatArray2DBool : FlatArray2D<bool> { }

		public FlatArray2DBool tiles = new FlatArray2DBool ();
		public List<Point2D> victoryTiles = new List<Point2D> ();
		public List<DogBlueprint> dogs = new List<DogBlueprint> ();
		public List<CatBlueprint> cats = new List<CatBlueprint> ();

		public int widthDisplay = 0;
		public int lengthDisplay = 0;

		public void RefreshDimensionDisplay () {
			widthDisplay = tiles.GetLength (0);
			lengthDisplay = tiles.GetLength (1);
		}

		public static LevelBlueprint DefaultLevel () {
			LevelBlueprint lbp = new LevelBlueprint ();
			lbp.tiles = new FlatArray2DBool ();
			lbp.tiles.Set2DShallow (new bool [0, 0].ChangedDimensions (10, 10, true));
			lbp.victoryTiles = new List<Point2D> ();
			lbp.dogs = new List<DogBlueprint> ();
			lbp.cats = new List<CatBlueprint> ();
			lbp.RefreshDimensionDisplay ();
			return lbp;
		}

		public static LevelBlueprint EmptyLevel () {
			LevelBlueprint lbp = new LevelBlueprint ();
			lbp.tiles = new FlatArray2DBool ();
			lbp.tiles.Set2DShallow (new bool [0, 0]);
			lbp.victoryTiles = new List<Point2D> ();
			lbp.dogs = new List<DogBlueprint> ();
			lbp.cats = new List<CatBlueprint> ();
			lbp.RefreshDimensionDisplay ();
			return lbp;
		}
	}
}