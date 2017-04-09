using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dog equivalent to CatExecutePhase.
/// Exits: DogSelectorPhase, PlayerTurnIdlePhase
/// </summary>
public class DogMovePhase : GameControlPhase {
	/// <summary>
	/// Used for static TakeControl
	/// </summary>
	private static DogMovePhase staticInstance;
	/// <summary>
	/// Puts the DogMovePhase in control.
	/// </summary>
	public static void TakeControl (Dog selectedDog) {
		staticInstance.selectedDog = selectedDog;
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}

	/// <summary>
	/// The dog that will move.
	/// </summary>
	private Dog selectedDog;

	/// <summary>
	/// The dog will follow this path.
	/// </summary>
	private List<Tile> tilePath;

	/// <summary>
	/// True only if the dog has not yet selected a path.
	/// </summary>
	private bool selecting;

	/// <summary>
	/// Creates the path the dog will walk.
	/// </summary>
	private void BuildPath () {
		PathingNode currentNode = selectedDog.myTile.pathingNode.SelectNextPathStart (selectedDog);
		PathingNode prevNode = selectedDog.lastVisited;
		while (!currentNode.isStoppingPoint) {
			tilePath.Add (currentNode.myTile);
			currentNode = currentNode.NextOnPath (prevNode);
		}
	}

	override public void OnTakeControl () {
		selecting = true;
		CameraOverheadControl.SetCamFollowTarget (selectedDog.transform);
	}

	override public void ControlUpdate () {
		if (selecting) {
			selecting = false;
			PathingNode nextNode = selectedDog.myTile.pathingNode.SelectNextPathStart (selectedDog);
			if (nextNode != null) {
				selectedDog.MoveTo (nextNode.myTile);
			}
			else {
				selectedDog.MoveTo (selectedDog.myTile.pathingNode.NextOnPath (selectedDog.lastVisited).myTile);
			}
		}
		else if (!selectedDog.myTile.pathingNode.isStoppingPoint) {
			Tile next = selectedDog.myTile.pathingNode.NextOnPath (selectedDog.lastVisited).myTile;
			if (next.occupant == null) {
				selectedDog.MoveTo (selectedDog.myTile.pathingNode.NextOnPath (selectedDog.lastVisited).myTile);
			}
			else {
				EndDogMovement ();
			}
		}
		else if (selectedDog.myTile.pathingNode.NextOnPath (selectedDog.lastVisited) == null) {
			EndDogMovement ();
		}
	}

	/// <summary>
	/// Ends the dog movement and decides what to do next. Includes detection check and advancing the phase.
	/// </summary>
	private void EndDogMovement () {
		Cat[] allCats = GameBrain.catManager.allCharacters;
		foreach (Cat c in allCats) {
			if (c.inDanger) {
				if (c.DetectionCheck (selectedDog)) {
					GameBrain.catManager.Remove (c);
					// destroy cat
				}
				else {
					c.ClearDangerByDog (selectedDog);
				}
			}
		}

		if (GameBrain.dogManager.availableCharacters.Length == 1) {
			CameraOverheadControl.SetCamFocusPoint (GameBrain.catManager.allCharacters.RandomElement ().myTile.topCenterPoint);
			GameBrain.dogManager.RejuvenateAll ();
			GameBrain.catManager.RejuvenateAll ();
			UIManager.masterInfoBox.ClearAllData ();
			UIManager.masterInfoBox.headerText = "";
			PlayerTurnIdlePhase.TakeControl ();
		}
		else {
			selectedDog.grayedOut = true;
			DogSelectorPhase.TakeControl ();
		}

	}
}
