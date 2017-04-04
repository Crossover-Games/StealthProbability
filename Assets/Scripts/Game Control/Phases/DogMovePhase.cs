using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dog equivalent to CatExecutePhase.
/// </summary>
public class DogMovePhase : GameControlPhase {

	/// <summary>
	/// a hack
	/// </summary>
	[SerializeField] private HACKCatReqtPhase reqt;

	/// <summary>
	/// The dog selector phase. No info required.
	/// </summary>
	[SerializeField] private DogSelectorPhase dogSelectorPhase;

	/// <summary>
	/// The player turn idle phase.
	/// </summary>
	[SerializeField] private PlayerTurnIdlePhase playerTurnIdlePhase;

	/// <summary>
	/// The dog that will move.
	/// </summary>
	public Dog selectedDog;

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
		PathingNode currentNode = selectedDog.myTile.pathingNode.SelectNextPathStart (selectedDog.lastVisited);
		PathingNode prevNode = selectedDog.lastVisited;
		while (!currentNode.isStoppingPoint) {
			tilePath.Add (currentNode.myTile);
			currentNode = currentNode.NextOnPath (prevNode);
		}
	}

	override public void OnTakeControl () {
		selecting = true;
		brain.cameraControl.SetCamFollowTarget (selectedDog.transform);
	}

	override public void ControlUpdate () {
		if (selecting) {
			selecting = false;
			PathingNode nextNode = selectedDog.myTile.pathingNode.SelectNextPathStart (selectedDog.lastVisited);
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

		bool HACK = false;

		Cat[] allCats = brain.catManager.allCharacters;
		foreach (Cat c in allCats) {
			if (c.inDanger) {
				if (c.DetectionCheck (selectedDog)) {
					brain.catManager.Remove (c);
					//GameObject.Destroy (c.gameObject);
					reqt.rektCat = c;
					reqt.TakeControl ();
					HACK = true;
				}
				else {
					c.ClearDangerByDog (selectedDog);
				}
			}
		}

		if (!HACK) {

			if (brain.dogManager.availableCharacters.Length == 1) {
				brain.cameraControl.SetCamFocusPoint (brain.catManager.allCharacters.RandomElement ().myTile.topCenterPoint);
				brain.dogManager.RejuvenateAll ();
				brain.catManager.RejuvenateAll ();
				brain.uiManager.masterInfoBox.ClearAllData ();
				brain.uiManager.masterInfoBox.headerText = "";
				playerTurnIdlePhase.TakeControl ();
			}
			else {
				selectedDog.grayedOut = true;
				dogSelectorPhase.TakeControl ();
			}
		}
	}
}
