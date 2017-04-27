using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelBuilder {
	/// <summary>
	/// All things that will go into instantiating a dog from the editor.
	/// </summary>
	public class DogBlueprint {
		public string name;
		public Compass.Direction direction;
		public Point2D point;
		public bool pathFoldout;
		public PathNodeState [,] nodeMap;
		public Dog myDog;
		public DogBlueprint (string n, Compass.Direction d, int x, int z) {
			name = n;
			direction = d;
			point = new Point2D (x, z);

			pathFoldout = false;
			nodeMap = new PathNodeState [0, 0];
			
			myDog = null;
		}
	}
}