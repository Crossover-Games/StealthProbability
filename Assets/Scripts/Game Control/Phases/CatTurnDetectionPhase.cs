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
	public static void TakeControl (Cat selectedCat, bool immediateFail) {
		staticInstance.selectedCat = selectedCat;
		staticInstance.immediateFail = immediateFail;
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}
	private bool immediateFail;

	/// <summary>
	/// since there's only going to be one cat, this might as well be it
	/// </summary>
	private Cat selectedCat;
	private float lastRolledChance;

	private Queue<DetectionMatchup> allChecks;

	/// <summary>
	/// Was the cat caught after all the checks?
	/// </summary>
	private bool rekt;

	private void RemoveCat () {
		AnimationManager.AddAnimation (selectedCat.transform, new AnimationDestination (null, null, Vector3.zero, 1f, InterpolationMethod.SquareRoot));
		GameBrain.catManager.Remove (selectedCat);
	}

	override public void OnTakeControl () {
		rekt = false;
		allChecks = DetectionManager.AllChecks ();
	}
	override public void ControlUpdate () {
		if (immediateFail) {
			rekt = false;
			immediateFail = false;
			allChecks = new Queue<DetectionMatchup> ();
			RemoveCat ();
		}
		else if (allChecks.Count > 0) {
			DetectionMatchup currentCheck = allChecks.Dequeue ();
			currentCheck.CameraHalfway ();
			DetectionManager.SetConflictHighlight (currentCheck);
			rekt = rekt || DetectionMeter.ConductRollAndAnimate (currentCheck);
		}
		else if (rekt) {
			if (selectedCat.hasWildCard && !immediateFail) {
				OneShotProjectile.LaunchAtPosition (selectedCat.myTile.topCenterPoint);
				selectedCat.hasWildCard = false;
			}
			else {
				RemoveCat ();
			}
			rekt = false;
		}
		else {
			EndChecking ();
		}
	}

	/// <summary>
	/// Advances the phase when there are no more dogs to check against.
	/// </summary>
	private void EndChecking () {
		if (rekt) {
			selectedCat.gameObject.SetActive (false);
			LosePhase.TakeControl ();
		}
		if (VictoryTile.gameWon) {
			VictoryPhase.TakeControl ();
		}
		else {
			selectedCat.DecrementWetTurns ();
			PlayerTurnIdlePhase.TakeControl ();
			Cat [] availableCharacters = GameBrain.catManager.availableCharacters;
			if (availableCharacters.Length > 0) {
				CameraOverheadControl.SetCamFocusPoint (availableCharacters.RandomElement ().myTile.topCenterPoint);
			}
		}
	}
}
