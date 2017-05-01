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
	public static void TakeControl () {
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}

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

	override public void OnTakeControl () {
		rekt = false;
		allChecks = DetectionManager.AllChecks ();
	}
	override public void ControlUpdate () {
		if (allChecks.Count > 0) {
			DetectionMatchup currentCheck = allChecks.Dequeue ();
			selectedCat = currentCheck.catInDanger;
			currentCheck.CameraHalfway ();
			DetectionManager.SetConflictHighlight (currentCheck);
			bool checkResult = currentCheck.SimulateDetectionCheck (out lastRolledChance);
			rekt = rekt || checkResult;
			DetectionMeter.AnimateRoll (currentCheck.danger, lastRolledChance, checkResult);
		}
		else if (rekt) {
			AnimationManager.AddAnimation (selectedCat.transform, new AnimationDestination (null, null, Vector3.zero, 1f, InterpolationMethod.SquareRoot));
			GameBrain.catManager.Remove (selectedCat);
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
		}

		if (VictoryTile.gameWon) {
			VictoryPhase.TakeControl ();
		}
		else if (VictoryTile.gameLost) {
			LosePhase.TakeControl ();
		}
		else {
			PlayerTurnIdlePhase.TakeControl ();
			Cat [] availableCharacters = GameBrain.catManager.availableCharacters;
			if (availableCharacters.Length > 0) {
				CameraOverheadControl.SetCamFocusPoint (availableCharacters.RandomElement ().myTile.topCenterPoint);
			}
		}
	}
}
