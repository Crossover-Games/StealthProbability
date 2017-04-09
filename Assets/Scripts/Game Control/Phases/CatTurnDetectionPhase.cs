using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The dog who most recently moved performs a detection check on all the cats it passed.
/// Exits: VictoryPhase, LosePhase, PlayerTurnIdlePhase
/// </summary>
public class CatTurnDetectionPhase : GameControlPhase {
	/// <summary>
	/// Used for static TakeControl
	/// </summary>
	private static CatTurnDetectionPhase staticInstance;
	/// <summary>
	/// Puts the CatTurnDetectionPhase in control.
	/// </summary>
	public static void TakeControl (Cat selectedCat) {
		staticInstance.selectedCat = selectedCat;
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}

	/// <summary>
	/// The cat to be checked.
	/// </summary>
	private Cat selectedCat;

	/// <summary>
	/// All dogs observing this cat.
	/// </summary>
	private List<Dog> dogsWatching;

	/// <summary>
	/// Was the cat caught after all the checks?
	/// </summary>
	private bool safe;

	override public void OnTakeControl () {
		safe = true;
		dogsWatching = selectedCat.dogsCrossed.ToList ();
	}
	override public void ControlUpdate () {
		if (dogsWatching.Count > 0) {
			Dog d = dogsWatching[0];
			//CameraOverheadControl.SetCamFocusPoint (d.myTile.topCenterPoint);
			if (selectedCat.DetectionCheckAndRemove (d)) {
				safe = false;
			}
			dogsWatching.RemoveAt (0);
		}
		else {
			EndChecking ();
		}
	}

	/// <summary>
	/// Advances the phase when there are no more dogs to check against.
	/// </summary>
	private void EndChecking () {
		if (!safe) {
			selectedCat.gameObject.SetActive (false);
		}

		if (VictoryTile.gameWon) {
			VictoryPhase.TakeControl ();
		}
		else if (VictoryTile.gameLost) {
			LosePhase.TakeControl ();
		}
		else {
			PlayerTurnIdlePhase.TakeControl ();
		}
	}
}
