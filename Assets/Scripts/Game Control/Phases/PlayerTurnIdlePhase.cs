using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This phase is when you're just looking around the map, planning out your turn. Leads into the DrawArrowPhase and dog turn.
/// </summary>
public class PlayerTurnIdlePhase : GameControlPhase {
	// override public void OnTakeControl ()	// should update the loose camera follow target to some custom point you control

	/// <summary>
	/// Exit node to draw arrow phase. Needs to know the target cat.
	/// </summary>
	[SerializeField] private DrawArrowPhase drawArrowPhase;

	/// <summary>
	/// Exit node demo pathing phase.
	/// </summary>
	[SerializeField] private DemoPathingPhase demoPathingPhase;


	private bool listenForMouseChange = false;
	private Vector3 previousMousePosition = Vector3.zero;
	private Tile clickedTile = null;

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

	override public void ControlUpdate () {
		if (listenForMouseChange) {
			if (!Input.GetMouseButton (0)) {
				listenForMouseChange = false;
			}
			else if (previousMousePosition != Input.mousePosition) {	//mouse dragged
				ExitToDrawArrowPhase ();
			}
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			brain.tileManager.cursorTile = null;
			demoPathingPhase.TakeControl ();
		}
	}

	private void ExitToDrawArrowPhase () {
		drawArrowPhase.selectedCat = clickedTile.occupant as Cat;
		drawArrowPhase.TakeControl ();
	}

	override public void OnLeaveControl () {
		listenForMouseChange = false;
	}
}
