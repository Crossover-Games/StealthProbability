using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This phase is when you're just looking around the map, planning out your turn. Leads into the DrawArrowPhase and dog turn.
/// </summary>
public class PlayerTurnIdlePhase : GameControlPhase {
	/// <summary>
	/// Exit node to draw arrow phase. Needs to know the target cat.
	/// </summary>
	[SerializeField] private DrawArrowPhase drawArrowPhase;

	/// <summary>
	/// Exit node to dog turn phase.
	/// </summary>
	[SerializeField] private DogTurnPhase dogTurnPhase;

	private bool catsAvailable = true;
	/// <summary>
	/// All cats.
	/// </summary>
	private List<Cat> allCats;

	[Tooltip("Parent of all cats in the scene.")]
	[SerializeField] private GameObject catParent;

	private bool listenForMouseChange = false;
	private Vector3 previousMousePosition = Vector3.zero;
	private Tile clickedTile = null;

	void Awake () {
		allCats = new List<Cat> (catParent.GetComponentsInChildren<Cat> ());
	}

	/// <summary>
	/// Called by GameBrain when the left mouse button goes down while the mouse is over a tile. Not called if the tile is null.
	/// </summary>
	override public void TileClickEvent (Tile t) {
		brain.tileManager.cursorTile = t;

		// once we have the holy grail info box working, we can put stuff like that in there too.

		// if t.occupant is a cat, wait for the user to move the mouse. That would indicate a drag and move to the draw arrow phase.
		if (t.occupant != null && t.occupant.characterType == CharacterType.Cat) {
			listenForMouseChange = true;
			previousMousePosition = Input.mousePosition;
			clickedTile = t;
		}
	}

	override public void OnTakeControl () {	// should update the loose camera follow target to some custom point you control
		CheckCatsAvailable ();
	}

	override public void ControlUpdate () {
		if (!catsAvailable) {
			dogTurnPhase.TakeControl ();
		}
		// checks for mouse drag
		else if (listenForMouseChange) {
			if (!Input.GetMouseButton (0)) {
				listenForMouseChange = false;
			}
			else if (previousMousePosition != Input.mousePosition) {	//mouse dragged
				ExitToDrawArrowPhase ();
			}
		}
	}

	private void ExitToDrawArrowPhase () {
		drawArrowPhase.selectedCat = clickedTile.occupant as Cat;
		drawArrowPhase.TakeControl ();
	}

	override public void OnLeaveControl () {
		listenForMouseChange = false;
	}

	private void CheckCatsAvailable () {
		catsAvailable = false;
		foreach (Cat c in allCats) {
			if (c.ableToMove) {
				catsAvailable = true;
			}
		}
	}

	public void RejuvenateAllCats () {
		foreach (Cat c in allCats) {
			c.ableToMove = true;
			c.isGrayedOut = false;
		}
		catsAvailable = true;
	}
}
