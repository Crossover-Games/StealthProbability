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
		GameBrain.dogManager.Shuffle ();
		activeDogSelecting = true;
		CameraOverheadControl.SetCamFollowTarget (GameBrain.dogManager.availableCharacters [0].transform);
	}

	override public void ControlUpdate () {
		if (GameBrain.dogManager.anyAvailable) {
			Dog currentDog = GameBrain.dogManager.availableCharacters [0];

			if (activeDogSelecting) {
				activeDogSelecting = false;
				currentDog.MoveTo (currentDog.myTile.pathingNode.SelectNextPathStart (currentDog).myTile);
			}
			else if (!currentDog.myTile.pathingNode.isStoppingPoint) {
				// TEMPORARY CHECKING FOR BLOCKED PASSAGE
				if (currentDog.myTile.pathingNode.NextOnPath (currentDog.lastVisited).myTile.occupant != null) {
					PathingNode last = currentDog.myTile.pathingNode;
					PathingNode current = currentDog.myTile.pathingNode.NextOnPath (currentDog.lastVisited);
					while (current != null) {
						PathingNode tempLast = current;
						current = current.NextOnPath (last);
						last = tempLast;
					}
					currentDog.MoveTo (last.myTile);
				}
				currentDog.MoveTo (currentDog.myTile.pathingNode.NextOnPath (currentDog.lastVisited).myTile);
			}
			else {
				EndCurrentDogMovement();
			}
		}
		else {
			GameBrain.dogManager.RejuvenateAll ();
			GameBrain.catManager.RejuvenateAll ();
			playerTurnIdlePhase.TakeControl ();
		}			
	}

	/// <summary>
	/// Ends the current dog movement.
	/// </summary>
	private void EndCurrentDogMovement(){
		GameBrain.dogManager.availableCharacters [0].grayedOut = true;
		if (GameBrain.dogManager.anyAvailable) {
			CameraOverheadControl.SetCamFollowTarget (GameBrain.dogManager.availableCharacters [0].transform);
		}
		activeDogSelecting = true;
	}

	override public void OnLeaveControl () {
		CameraOverheadControl.SetCamFocusPoint (GameBrain.catManager.allCharacters.RandomElement ().myTile.topCenterPoint);
	}
}
