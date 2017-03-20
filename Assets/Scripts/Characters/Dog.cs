using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : GameCharacter {
	override public CharacterType characterType {
		get{ return CharacterType.Dog; }
	}

	override public float stepAnimationTime {
		get { return 0.5f; }
	}

	public PathingNode lastVisited = null;

	/// <summary>
	/// Doggoveride. In addition to GameCharacter.MoveTo(Tile), this stores the pathing node of the last tile it visited.
	/// that's not implemented yet.
	/// </summary>
	override public void MoveTo (Tile destination) {
		if (Tile.IsValidMoveDestination (destination)) {
			lastVisited = myTile.pathingNode;
		}
		base.MoveTo (destination);
	}

	//also has vision pattern

	/// <summary>
	/// For demonstration purposes, this dog moves to the next node in a hard coded ring.
	/// </summary>
	public void DemoMoveAlongTrack () {
		MoveTo (myTile.pathingNode.NextOnPath (lastVisited).myTile);
	}
}
