using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In this phase, the cat just carries out the orders it was given.
/// </summary>
public class CatExecutePhase : GameControlPhase {
	// override public void OnTakeControl ()
	// override public void OnLeaveControl ()
	// override public void TileClickEvent (Tile t)

	/// <summary>
	/// Exit node to player turn idle phase.
	/// </summary>
	[SerializeField] private PlayerTurnIdlePhase playerTurnIdlePhase;

	/// <summary>
	/// Target cat.
	/// </summary>
	[HideInInspector] public Cat selectedCat;

	/// <summary>
	/// The ordered path the cat will take.
	/// </summary>
	[HideInInspector] public List<Tile> tilePath;

	override public void ControlUpdate (){
		if (tilePath.Count > 0) {
			selectedCat.MoveTo (tilePath [0]);
			tilePath.RemoveAt (0);
		}
		else {
			playerTurnIdlePhase.TakeControl ();
		}
	}
}
