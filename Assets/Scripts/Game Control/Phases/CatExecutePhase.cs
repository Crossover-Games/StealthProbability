using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In this phase, the cat just carries out the orders it was given.
/// </summary>
public class CatExecutePhase : GameControlPhase {

	/// <summary>
	/// Exit node to player turn idle phase.
	/// </summary>
	[SerializeField] private PlayerTurnIdlePhase playerTurnIdlePhase;

	/// <summary>
	/// Target cat.
	/// </summary>
	[HideInInspector] public Cat selectedCat;

	/// <summary>
	/// The ordered path the cat will take.
	/// </summary>
	[HideInInspector] public List<Floor> tilePath;


	[SerializeField] private AudioSource purrSound;

	override public void OnTakeControl () {
		CameraOverheadControl.SetCamFollowTarget (selectedCat.transform);
		purrSound.Play ();
		selectedCat.walkingAnimation = true;
	}

	override public void ControlUpdate () {
		if (tilePath.Count > 0) {
			selectedCat.MoveTo (tilePath [0]);
			tilePath.RemoveAt (0);
		}
		else {
			EndMovement ();
		}
	}

	/// <summary>
	/// Ends the movement. Does detection checks.
	/// </summary>
	private void EndMovement () {

		Dog[] tempDogs = selectedCat.dogsCrossed.ToArray ();
		for (int x = 0; x < tempDogs.Length; x++) {
			if (selectedCat.DetectionCheck (tempDogs [x])) {
				GameBrain.catManager.Remove (selectedCat);
				// destroy the cat in some way
			}
			else {
				selectedCat.ClearDangerByDog (tempDogs [x]);
			}
		}
		playerTurnIdlePhase.TakeControl ();
	}

	override public void OnLeaveControl () {
		CameraOverheadControl.StopFollowing ();
		purrSound.Stop ();
		selectedCat.grayedOut = true;
		selectedCat.walkingAnimation = false;
	}
}
