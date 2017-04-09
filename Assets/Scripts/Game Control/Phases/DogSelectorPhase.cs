using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The equivalent to PlayerTurnIdlePhase for dogs. Simply selects which dog to move, then gives control over to the phase that moves the dog.
/// Exits: DogMovePhase
/// </summary>
public class DogSelectorPhase : GameControlPhase {
	/// <summary>
	/// Used for static TakeControl
	/// </summary>
	private static DogSelectorPhase staticInstance;
	/// <summary>
	/// Puts the DogSelectorPhase in control.
	/// </summary>
	public static void TakeControl () {
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}
	override public void ControlUpdate () {
		if (GameBrain.dogManager.anyAvailable) {
			UIManager.masterInfoBox.ClearAllData ();
			Dog nextDog = GameBrain.dogManager.availableCharacters[0];
			UIManager.masterInfoBox.headerText = nextDog.name;
			DogMovePhase.TakeControl (nextDog);
		}
	}
}
