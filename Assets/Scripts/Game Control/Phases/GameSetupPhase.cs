using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The first phase. Does some setup maintenanace and then passes on the turn to the player.
/// Exits: PlayerTurnIdlePhase
/// </summary>
public class GameSetupPhase : GameControlPhase {
	/// <summary>
	/// Used for static TakeControl
	/// </summary>
	private static GameSetupPhase staticInstance;
	/// <summary>
	/// Puts the GameSetupPhase in control
	/// </summary>
	public static void TakeControl () {
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}
	/// <summary>
	/// Displays dog vision patterns.
	/// </summary>
	public override void OnTakeControl () {
		foreach (Dog dog in GameBrain.dogManager.allCharacters) {
			dog.ApplyVisionPattern ();
		}
		CameraOverheadControl.SetCamInstantPoint (GameBrain.catManager.allCharacters.RandomElement ().myTile.topCenterPoint);
	}

	public override void ControlUpdate () {
		PlayerTurnIdlePhase.TakeControl ();
	}
}
