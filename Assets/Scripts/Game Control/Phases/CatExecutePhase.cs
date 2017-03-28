using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In this phase, the cat just carries out the orders it was given.
/// </summary>
public class CatExecutePhase : GameControlPhase {

	/// <summary>
	/// a hack
	/// </summary>
	[SerializeField] private HACKCatReqtPhase reqt;


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
	[HideInInspector] public List<Tile> tilePath;


	[SerializeField] private AudioSource purrSound;

	override public void OnTakeControl () {
		brain.cameraControl.SetCamFollowTarget (selectedCat.transform);
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
		bool HACK = false;

		Dog[] tempDogs = selectedCat.dogsCrossed.ToArray ();
		for (int x = 0; x < tempDogs.Length; x++) {
			if (selectedCat.DetectionCheck (tempDogs [x])) {
				brain.catManager.Remove (selectedCat);
				//GameObject.Destroy (selectedCat.gameObject);

				// LEL WHAT A HACK

				reqt.rektCat = selectedCat;
				reqt.TakeControl ();
				HACK = true;
				//x = tempDogs.Length;
			}
			else {
				selectedCat.ClearDangerByDog (tempDogs [x]);
			}
		}

		if (!HACK) {
			playerTurnIdlePhase.TakeControl ();
		}
	}

	override public void OnLeaveControl () {
		brain.cameraControl.StopFollowing ();
		purrSound.Stop ();
		selectedCat.grayedOut = true;
		selectedCat.walkingAnimation = false;
	}
}
