using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for the main opposing forces.
/// </summary>
public class Dog : GameCharacter {
	override public CharacterType characterType {
		get { return CharacterType.Dog; }
	}

	override public float stepAnimationTime {
		get { return 0.5f; }
	}

	override public float stepRotationFraction {
		get { return 0.3f; }
	}

	private VisionPattern m_VisionPattern;
	/// <summary>
	/// This dog's vision pattern.
	/// </summary>
	public VisionPattern visionPattern {
		get { return m_VisionPattern; }
	}

	override protected void Awake () {
		base.Awake ();
		m_VisionPattern = new VisionPattern (this);
	}

	[SerializeField] private Route m_route;
	/// <summary>
	/// The entire route this dog can take.
	/// </summary>
	public Route route {
		get { return m_route; }
	}

	void Start () {
		m_route.SetGuaranteedPath (myTile.GetNeighborInDirection (orientation).stepNode.myPath);
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

	/// <summary>
	/// Doggoveride. In addition to GameCharacter.MoveTo(Tile), this applies the vision pattern.
	/// </summary>
	override public void MoveTo (Tile destination) {
		if (Tile.ValidStepDestination (destination)) {
			ClearVisionPattern ();
			base.MoveTo (destination);
			ApplyVisionPattern ();
		}
	}
}
