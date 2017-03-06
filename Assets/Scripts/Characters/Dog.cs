using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : GameCharacter {
	override public CharacterType characterType {
		get{ return CharacterType.Dog; }
	}

	public PathingNode lastVisited = null;

	/// <summary>
	/// PLACEHOLDER OVERRIDE. Stores the last visited pathing node, in addition to everything the parent method does.
	/// that's not implemented yet.
	/// </summary>
	override public void MoveTo (Tile destination) {
		if (destination != null) {
			lastVisited = myTile.pathingNode;

			Tile tmp = myTile;
			tmp.SetOccupant (null);

			myTile = destination;
			transform.position = myTile.characterConnectionPoint;
		}
	}

	//also has vision pattern

	/// <summary>
	/// For demonstration purposes, this dog moves to the next node in a hard coded ring.
	/// </summary>
	public void DemoMoveAlongTrack () {
		MoveTo (myTile.pathingNode.nextOnPath (lastVisited).myTile);
	}
}
