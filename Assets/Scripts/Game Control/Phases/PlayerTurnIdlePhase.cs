using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This phase is when you're just looking around the map, planning out your turn. Leads into the DrawArrowPhase and dog turn.
/// </summary>
public class PlayerTurnIdlePhase : GameControlPhase {
	// override public void OnTakeControl ()	// should update the loose camera follow target to some custom point you control
	// override public void OnLeaveControl ()
	// override public void ControlUpdate ()

	/// <summary>
	/// DrawArrowPhase object. Used for moving into the next phase and passing data beforehand.
	/// </summary>
	[SerializeField] private DrawArrowPhase drawArrowPhase;

	/// <summary>
	/// The demo pathing phase.
	/// </summary>
	[SerializeField] private DemoPathingPhase demoPathingPhase;

	/// <summary>
	/// Called by GameBrain when the left mouse button goes down while the mouse is over a tile. Not called if the tile is null.
	/// </summary>
	override public void TileClickEvent (Tile t) {
		brain.tileManager.cursorTile = t;

		// once we have the holy grail info box working, we can put stuff like that in there too.

		// if t.occupant is a cat, move into draw arrow phase.
		if (t.occupant != null && t.occupant.characterType == CharacterType.Cat) {
			drawArrowPhase.selectedCat = t.occupant;
			drawArrowPhase.TakeControl ();
		}
	}

	override public void ControlUpdate () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			brain.tileManager.cursorTile = null;
			demoPathingPhase.TakeControl ();
		}
	}
}
