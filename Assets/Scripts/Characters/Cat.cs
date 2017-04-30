using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : GameCharacter {

	override public CharacterType characterType {
		get { return CharacterType.Cat; }
	}

	override public float stepAnimationTime {
		get { return 0.5f; }
	}

	override public float stepRotationFraction {
		get { return 0.3f; }
	}

	[SerializeField] private int m_maxEnergy;
	/// <summary>
	/// The measure for how much a cat can do in a turn. Converts to movement, actions, and extra stealth.
	/// </summary>
	public int maxEnergy {
		get { return m_maxEnergy; }
	}

	/// <summary>
	/// Decreased by one on detection. If this hits zero, this cat is GONE
	/// </summary>
	public int livesRemaining = 2;

	/// <summary>
	/// Moves and gathers danger.
	/// </summary>
	override public void MoveTo (Tile destination) {
		base.MoveTo (destination);

		if (Tile.ValidStepDestination (destination)) {
			TileDangerData [] dangerArray = myTile.dangerData;
			if (dangerArray.Length > 0) {
				DetectionManager.AddDanger (this, dangerArray);
			}
		}
	}

	public bool walkingAnimation {
		get { return animator.GetBool ("Moving"); }
		set { animator.SetBool ("Moving", value); }
	}
}
