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

	/// <summary>
	/// Doggoveride. In addition to GameCharacter.MoveTo(Tile), this stores the pathing node of the last tile it visited.
	/// that's not implemented yet.
	/// </summary>
	override public void MoveTo (Tile destination) {
		if (UniversalTileManager.IsValidMoveDestination (destination)) {
			ClearVisionPattern ();
			lastVisited = myTile.pathingNode;
		}
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
