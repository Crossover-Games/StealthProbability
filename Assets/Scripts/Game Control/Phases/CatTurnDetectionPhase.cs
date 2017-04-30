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

	private Cat selectedCat;

	private Queue<DetectionMatchup> allChecks;

	/// <summary>
	/// Was the cat caught after all the checks?
	/// </summary>
	private bool safe;

	override public void OnTakeControl () {
		safe = true;
		allChecks = DetectionManager.AllChecks ();
	}
	override public void ControlUpdate () {
		if (allChecks.Count > 0) {
			DetectionMatchup currentCheck = allChecks.Dequeue ();
			selectedCat = currentCheck.catInDanger;
			currentCheck.CameraHalfway ();
			DetectionManager.SetConflictHighlight (currentCheck);
			if (currentCheck.SimulateDetectionCheck ()) {
				AnimationManager.AddAnimation (currentCheck.catInDanger.transform, new AnimationDestination (null, null, Vector3.zero, 1f, InterpolationMethod.SquareRoot));
				GameBrain.catManager.Remove (currentCheck.catInDanger);
				safe = false;
			}
			else {
				AnimationManager.DummyTime (1f);
			}
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
			Cat [] availableCharacters = GameBrain.catManager.availableCharacters;
			if (availableCharacters.Length > 0) {
				CameraOverheadControl.SetCamFocusPoint (availableCharacters.RandomElement ().myTile.topCenterPoint);
			}
		}
	}
}
