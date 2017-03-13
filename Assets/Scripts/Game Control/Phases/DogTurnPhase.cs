using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dogs just move around and do dog things in this phase.
/// </summary>
public class DogTurnPhase : GameControlPhase {
	// override public void OnLeaveControl ()
	// override public void ControlUpdate ()
	// override public void TileClickEvent (Tile t)
	// override public void MouseOverChangeEvent ()

	/// <summary>
	/// Exit node to player turn idle phase.
	/// </summary>
	[SerializeField] private PlayerTurnIdlePhase playerTurnIdlePhase;

	/// <summary>
	/// All dogs.
	/// </summary>
	private List<Dog> allDogs;

	private List<Dog> activeDogs;
	/// <summary>
	/// True if this is the absolute first time it's moving this turn. Useful for selecting a path.
	/// </summary>
	private bool activeDogSelecting;

	[Tooltip("Parent of all dogs in the scene.")]
	[SerializeField] private GameObject dogParent;

	void Awake () {
		allDogs = new List<Dog> (dogParent.GetComponentsInChildren<Dog> ());
	}

	override public void OnTakeControl () {
		activeDogs = new List<Dog> (allDogs);
		activeDogSelecting = true;
		brain.cameraControl.SetCamFollowTarget (activeDogs [0].transform);
	}

	override public void ControlUpdate () {
		if (activeDogs.Count != 0) {
			if (activeDogSelecting) {
				activeDogSelecting = false;
				activeDogs [0].MoveTo (activeDogs [0].myTile.pathingNode.SelectNextPath (activeDogs [0].lastVisited).myTile);
			}
			else if (!activeDogs [0].myTile.pathingNode.isStoppingPoint) {
				activeDogs [0].MoveTo (activeDogs [0].myTile.pathingNode.NextOnPath (activeDogs [0].lastVisited).myTile);
			}
			else {
				activeDogs [0].isGrayedOut = true;
				activeDogs.RemoveAt (0);
				if (activeDogs.Count != 0) {
					brain.cameraControl.SetCamFollowTarget (activeDogs [0].transform);
				}
				activeDogSelecting = true;
			}
		}
		else {
			foreach (Dog d in allDogs) {
				d.isGrayedOut = false;
			}
			playerTurnIdlePhase.RejuvenateAllCats ();
			playerTurnIdlePhase.TakeControl ();
		}			
	}
}
