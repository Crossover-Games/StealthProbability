using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The equivalent to PlayerTurnIdlePhase for dogs. Simply selects which dog to move, then gives control over to the phase that moves the dog.
/// </summary>
public class DogSelectorPhase : GameControlPhase {

	[SerializeField] private DogMovePhase dogMovePhase;

	override public void ControlUpdate () {
		if (brain.dogManager.anyAvailable) {
			dogMovePhase.selectedDog = brain.dogManager.availableCharacters [0];
			dogMovePhase.TakeControl ();
		}
	}
}
