using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for the main opposing forces.
/// </summary>
public class Dog : GameCharacter {
	override public CharacterType characterType {
		get{ return CharacterType.Dog; }
	}

	override public float stepAnimationTime {
		get { return 0.5f; }
	}

	public PathingNode lastVisited = null;

	[SerializeField] private VisionPattern m_VisionPattern;
	/// <summary>
	/// This dog's vision pattern.
	/// </summary>
	public VisionPattern visionPattern {
		get{ return m_VisionPattern; }
	}

	/// <summary>
	/// Doggoveride. In addition to GameCharacter.MoveTo(Tile), this stores the pathing node of the last tile it visited.
	/// that's not implemented yet.
	/// </summary>
	override public void MoveTo (Tile destination) {
		if (UniversalTileManager.IsValidMoveDestination (destination)) {
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
