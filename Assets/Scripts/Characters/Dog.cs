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

	override public float stepRotationFraction {
		get { return 0.3f; }
	}

	public PathingNode lastVisited = null;

	private PathingNode m_firstTurnNode;
	/// <summary>
	/// The dog only moves forward on the first turn. Null after the dog makes its first move.
	/// </summary>
	public PathingNode firstTurnNode {
		get { return m_firstTurnNode; }
	}

	private VisionPattern m_VisionPattern;
	/// <summary>
	/// This dog's vision pattern.
	/// </summary>
	public VisionPattern visionPattern {
		get{ return m_VisionPattern; }
	}

	override protected void Awake () {
		base.Awake ();
		m_VisionPattern = new VisionPattern (this);
	}

	void Start () {
		Tile tempTile = myTile.GetNeighborInDirection (orientation);
		if (tempTile != null) {
			m_firstTurnNode = tempTile.pathingNode;
		}
	}

	/// <summary>
	/// Doggoveride. In addition to GameCharacter.MoveTo(Tile), this stores the pathing node of the last tile it visited.
	/// that's not implemented yet.
	/// </summary>
	override public void MoveTo (Tile destination) {
		if (Tile.ValidStepDestination (destination)) {
			ClearVisionPattern ();
			lastVisited = myTile.pathingNode;
		}
		m_firstTurnNode = null;
		base.MoveTo (destination);
		ApplyVisionPattern ();
	}

	/// <summary>
	/// Applies the vision pattern to the ground. If any tile is under a cat, register to that cat.
	/// </summary>
	public void ApplyVisionPattern () {
		foreach (TileDangerData tdd in m_VisionPattern.allTilesAffected) {
			tdd.myTile.AddDangerData (tdd);
		}
	}

	/// <summary>
	/// Lifts this dog's vision pattern from the ground.
	/// </summary>
	public void ClearVisionPattern () {
		foreach (TileDangerData tdd in m_VisionPattern.allTilesAffected) {
			tdd.myTile.RemoveDangerDataByDog (this);
		}
	}
}
