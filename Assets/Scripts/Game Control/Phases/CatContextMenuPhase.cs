using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// During this phase, the player has already selected a path for a certain cat to move. This phase waits for the player to decide to rest, cancel, or use an action.
/// </summary>
public class CatContextMenuPhase : GameControlPhase {
	/// <summary>
	/// Exit node to player turn idle phase.
	/// </summary>
	[SerializeField] private PlayerTurnIdlePhase playerTurnIdlePhase;

	/// <summary>
	/// Exit node to cat execute phase. Needs to know the target cat and the path.
	/// </summary>
	[SerializeField] private CatExecutePhase catExecutePhase;

	// Should also have exit node to the action targeting phase

	/// <summary>
	/// Target cat.
	/// </summary>
	[HideInInspector] public Cat selectedCat;

	/// <summary>
	/// The ordered path the cat will take.
	/// </summary>
	[HideInInspector] public List<Tile> tilePath;

	/*
	 * opens a menu with Rest, Cancel, and Action (no action for now)
	 * preferably, each of the buttons would have a clicked event which triggers an event here
	 *
	 */

	override public void OnTakeControl () {
		// show menu at cursor
	}

	/// <summary>
	/// when ya click cancel
	/// </summary>
	private void ExitToPlayerTurnIdlePhase () {
		playerTurnIdlePhase.TakeControl ();
	}

	override public void ControlUpdate (){
		// just jump straight into execute phase. No menu implemented yet.
		catExecutePhase.selectedCat = selectedCat;
		catExecutePhase.tilePath = tilePath;
		catExecutePhase.TakeControl ();
	}

	override public void OnLeaveControl () {
		// hide menu
	}
}
