using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A cluster of related states in the game control state machine grouped together to form a phase of game control.
/// </summary>
public abstract class GameControlPhase : MonoBehaviour {

	/// <summary>
	/// Puts this phase in control of the GameBrain. First, it calls the OnLeaveControl() of the previous phase, then calls this phase's OnTakeControl().
	/// </summary>
	public void InstanceTakeControl () {
		GameBrain.AssignControl (this);
	}

	/// <summary>
	/// This happens when this phase takes control, after the previous phase runs its OnLeaveControl.
	/// </summary>
	virtual public void OnTakeControl () {
		// by default, do nothing
	}

	/// <summary>
	/// This happens immediately before a new phase takes control.
	/// </summary>
	virtual public void OnLeaveControl () {
		// by default, do nothing
	}

	/// <summary>
	/// Like MonoBehaviour.Update(), but is only called when in control of the GameBrain.
	/// </summary>
	virtual public void ControlUpdate () {
		// by default, do nothing
	}

	/// <summary>
	/// Called by GameBrain when the left mouse button goes down while the mouse is over a tile. Not called if the tile is null or if this is a double click.
	/// </summary>
	virtual public void TileClickEvent (Tile t) {
		// by default, do nothing
	}

	/// <summary>
	/// Called by GameBrain when the a tile is dragged a certain distance.
	/// </summary>
	virtual public void TileDragEvent (Tile t) {
		// by default, do nothing
	}

	/// <summary>
	/// Called by GameBrain when the left mouse button goes down twice in rapid succession while the mouse is over a tile. Not called if the tile is null.
	/// </summary>
	virtual public void TileDoubleClickEvent (Tile t) {
		// by default, do nothing
	}

	/// <summary>
	/// Called by GameBrain when the mouse hovers over a different tile than before. Be mindful that this may mean the new "tile" is null if the mouse went off the map.
	/// </summary>
	virtual public void MouseOverChangeEvent () {
		// by default, do nothing
	}

	/// <summary>
	/// Called by GameBrain when the cancel path button is clicked.
	/// </summary>
	virtual public void UICancelPathButtonEvent () {
		// by default, do nothing
	}

	/// <summary>
	/// Called by GameBrain when the rest button is clicked.
	/// </summary>
	virtual public void UIRestButtonEvent () {
		// by default, do nothing
	}

	/// <summary>
	/// Called by GameBrain when the action button is clicked.
	/// </summary>
	virtual public void UIActionButtonEvent () {
		// by default, do nothing
	}
}
// override public void OnTakeControl ()
// override public void OnLeaveControl ()
// override public void ControlUpdate ()
// override public void TileClickEvent (Tile t)
// override public void TileDragEvent (Tile t)
// override public void TileDoubleClickEvent (Tile t)
// override public void MouseOverChangeEvent ()
