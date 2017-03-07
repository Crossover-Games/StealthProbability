using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : GameCharacter {

	override public CharacterType characterType {
		get{ return CharacterType.Cat; }
	}

	[SerializeField] private int myMaxEnergy;
	/// <summary>
	/// The measure for how much a cat can do in a turn. Converts to movement, actions, and extra stealth.
	/// </summary>
	public int maxEnergy {
		get{ return myMaxEnergy; }
	}

	/// <summary>
	/// Decreased by one on detection. If this hits zero, this cat is GONE
	/// </summary>
	public int livesRemaining = 2;
}
