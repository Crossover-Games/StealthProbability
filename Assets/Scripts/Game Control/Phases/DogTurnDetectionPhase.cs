using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The dog who most recently moved performs a detection check on all the cats it passed.
/// Exits: DogSelectorPhase, PlayerTurnIdlePhase
/// </summary>
public class DogTurnDetectionPhase : GameControlPhase {
	/// <summary>
	/// Used for static TakeControl
	/// </summary>
	private static DogTurnDetectionPhase staticInstance;
	/// <summary>
	/// Puts the DogTurnDetectionPhase in control.
	/// </summary>
	public static void TakeControl (Dog selectedDog) {
		staticInstance.selectedDog = selectedDog;
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}

	/// <summary>
	/// The dog to perform the check.
	/// </summary>
	private Dog selectedDog;

	/// <summary>
	/// All cats in danger.
	/// </summary>
	private List<Cat> catsInDanger;
	/// <summary>
	/// Rekt cats will be disabled.
	/// </summary>
	private List<Cat> catsToDisable;

	override public void OnTakeControl () {
		catsInDanger = new List<Cat> ();
		catsToDisable = new List<Cat> ();
		foreach (Cat c in GameBrain.catManager.allCharacters) {
			if (c.inDanger) {
				catsInDanger.Add (c);
			}
		}
	}
	override public void ControlUpdate () {
		if (catsInDanger.Count > 0) {
			Cat c = catsInDanger [0];
			CameraOverheadControl.SetCamFocusPoint (c.myTile.topCenterPoint);
			if (c.DetectionCheckAndRemove (selectedDog)) {
				catsToDisable.Add (c);
			}
			catsInDanger.RemoveAt (0);
		}
		else {
			EndChecking ();
		}
	}

	/// <summary>
	/// Advances the phase when there are no more cats to check.
	/// </summary>
	private void EndChecking () {
		foreach (Cat c in catsToDisable) {
			c.gameObject.SetActive (false);
		}
		DogSelectorPhase.TakeControl ();
	}
}
