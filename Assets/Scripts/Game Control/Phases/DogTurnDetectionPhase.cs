﻿using System.Collections;
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
	public static void TakeControl () {
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}

	/// <summary>
	/// Rekt cats will be disabled.
	/// </summary>
	private List<Cat> catsToDisable;

	private float lastRolledChance;
	private Cat lastCatRekt;

	private Queue<DetectionMatchup> allChecks;

	override public void OnTakeControl () {
		lastCatRekt = null;
		catsToDisable = new List<Cat> ();
		allChecks = DetectionManager.AllChecks ();
	}
	override public void ControlUpdate () {
		if (lastCatRekt != null) {
			if (lastCatRekt.hasWildCard) {
				OneShotProjectile.LaunchAtPosition (lastCatRekt.myTile.topCenterPoint);
				lastCatRekt.hasWildCard = false;
			}
			else {
				AnimationManager.AddAnimation (lastCatRekt.transform, new AnimationDestination (null, null, Vector3.zero, 1f, InterpolationMethod.SquareRoot));
				GameBrain.catManager.Remove (lastCatRekt);
			}
			lastCatRekt = null;
		}
		else if (allChecks.Count > 0) {
			DetectionMatchup currentCheck = allChecks.Dequeue ();
			currentCheck.CameraHalfway ();
			DetectionManager.SetConflictHighlight (currentCheck);
			bool checkResult = currentCheck.SimulateDetectionCheck (out lastRolledChance);
			if (checkResult) {
				lastCatRekt = currentCheck.catInDanger;
			}
			DetectionMeter.AnimateRoll (currentCheck.danger, lastRolledChance, checkResult, currentCheck.catInDanger);
		}
		else {
			EndChecking ();
		}
	}

	/// <summary>
	/// Advances the phase when there are no more cats to check.
	/// </summary>
	private void EndChecking () {
		if (VictoryTile.gameWon) {
			VictoryPhase.TakeControl ();
		}
		else if (VictoryTile.gameLost) {
			LosePhase.TakeControl ();
		}
		else {
			foreach (Cat c in catsToDisable) {
				c.gameObject.SetActive (false);
			}
			DogSelectorPhase.TakeControl ();
		}
	}
}
