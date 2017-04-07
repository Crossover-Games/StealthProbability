using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The first phase. Does some setup maintenanace and then passes on the turn to the player.
/// </summary>
public class GameSetupPhase : GameControlPhase {

	/// <summary>
	/// Exit node to player turn idle phase.
	/// </summary>
	[SerializeField] private PlayerTurnIdlePhase playerTurnIdlePhase;

	/// <summary>
	/// Displays dog vision patterns.
	/// </summary>
	public override void OnTakeControl () {
		foreach (Dog dog in GameBrain.dogManager.allCharacters) {
			dog.ApplyVisionPattern ();
		}
		foreach (Cat cat in GameBrain.catManager.allCharacters) {
			cat.ClearDangerData ();
		}
	}

	public override void ControlUpdate () {
		playerTurnIdlePhase.TakeControl ();
	}
}
