using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dogs just move around and do dog things in this phase.
/// </summary>
public class DogTurnPhase : GameControlPhase {
	/// <summary>
	/// Exit node to player turn idle phase.
	/// </summary>
	[SerializeField] private PlayerTurnIdlePhase playerTurnIdlePhase;

	/// <summary>
	/// True if this is the absolute first time it's moving this turn. Useful for selecting a path.
	/// </summary>
	private bool activeDogSelecting;

	override public void OnTakeControl () {
		brain.dogManager.Shuffle ();
		activeDogSelecting = true;
		brain.cameraControl.SetCamFollowTarget (brain.dogManager.availableCharacters [0].transform);
	}

	override public void ControlUpdate () {
		if (brain.dogManager.anyAvailable) {
			Dog currentDog = brain.dogManager.availableCharacters [0];

			if (activeDogSelecting) {
				activeDogSelecting = false;
				currentDog.MoveTo (currentDog.myTile.pathingNode.SelectNextPathStart (currentDog.lastVisited).myTile);
			}
			else if (!currentDog.myTile.pathingNode.isStoppingPoint) {
				currentDog.MoveTo (currentDog.myTile.pathingNode.NextOnPath (currentDog.lastVisited).myTile);
			}
			else {
				// Normally, don't bother graying out the last dog because it's just going to switch anyway. That's not done right now tho
				currentDog.grayedOut = true;
				if (brain.dogManager.anyAvailable) {
					brain.cameraControl.SetCamFollowTarget (brain.dogManager.availableCharacters [0].transform);
				}
				activeDogSelecting = true;
			}
		}
		else {
			brain.dogManager.RejuvenateAll ();
			brain.catManager.RejuvenateAll ();
			playerTurnIdlePhase.TakeControl ();
		}			
	}

	override public void OnLeaveControl () {
		brain.cameraControl.SetCamFocusPoint (brain.catManager.allCharacters.RandomElement ().myTile.topCenterPoint);
	}
}
