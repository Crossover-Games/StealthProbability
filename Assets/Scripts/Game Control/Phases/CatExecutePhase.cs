using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In this phase, the cat just carries out the orders it was given.
/// Exits: PlayerTurnIdlePhase
/// </summary>
public class CatExecutePhase : GameControlPhase {
	/// <summary>
	/// Used for static TakeControl
	/// </summary>
	private static CatExecutePhase staticInstance;
	/// <summary>
	/// Puts the CatExecutePhase in control
	/// </summary>
	public static void TakeControl (Cat selectedCat, List<Tile> tilePath) {
		staticInstance.selectedCat = selectedCat;
		staticInstance.tilePath = tilePath;
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}
	/// <summary>
	/// Target cat.
	/// </summary>
	private Cat selectedCat;

	/// <summary>
	/// The ordered path the cat will take.
	/// </summary>
	public List<Tile> tilePath;


	[SerializeField] private AudioSource purrSound;

	override public void OnTakeControl () {
		CameraOverheadControl.SetCamFollowTarget (selectedCat.transform);
		purrSound.Play ();
		selectedCat.walkingAnimation = true;
	}

	override public void ControlUpdate () {
		if (tilePath.Count > 0) {
			selectedCat.MoveTo (tilePath[0]);
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
			if (selectedCat.DetectionCheck (tempDogs[x])) {
				GameBrain.catManager.Remove (selectedCat);
				// destroy the cat in some way
			}
			else {
				selectedCat.ClearDangerByDog (tempDogs[x]);
			}
		}
		PlayerTurnIdlePhase.TakeControl ();
	}

	override public void OnLeaveControl () {
		CameraOverheadControl.StopFollowing ();
		purrSound.Stop ();
		selectedCat.grayedOut = true;
		selectedCat.walkingAnimation = false;
	}
}
