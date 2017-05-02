﻿using System.Collections;
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

	public override string flavorText {
		get { return "Grunt unit of the international fashion police."; }
	}

	[SerializeField] private int m_maxEnergy;
	/// <summary>
	/// The measure for how much a cat can do in a turn. Converts to movement, actions, and extra stealth.
	/// </summary>
	public int maxEnergy {
		get { return m_maxEnergy; }
	}

	/// <summary>
	/// Evade detection once.
	/// </summary>
	public bool hasWildCard = true;

	/// <summary>
	/// Moves and gathers danger.
	/// </summary>
	override public void MoveTo (Tile destination) {
		bool valid = Tile.ValidStepDestination (destination);
		if (valid) {
			if (destination.occupant != null) {
				CatExecutePhase.charactersCrossed.Push (destination.occupant);
			}
		}
		base.MoveTo (destination);
		if (valid) {
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
