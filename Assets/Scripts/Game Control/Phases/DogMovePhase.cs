using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dog equivalent to CatExecutePhase.
/// Exits: DogTurnCatDetectPhase
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
				DogTurnDetectionPhase.TakeControl (selectedDog);
			}
		}
		else if (selectedDog.myTile.pathingNode.NextOnPath (selectedDog.lastVisited) == null) {
			DogTurnDetectionPhase.TakeControl (selectedDog);
		}
	}

}
