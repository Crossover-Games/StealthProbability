using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game phase for when you're drawing the arrow to move a cat. Leads only into the ArrowMenuPhase.
/// </summary>
public class DrawArrowPhase : GameControlPhase {
	// override public void TileClickEvent (Tile t)

	/// <summary>
	/// The ordered path of all tiles for which the arrow follows.
	/// </summary>
	private List<Tile> tilePath;

	/// <summary>
	/// PlayerTurnIdlePhase object. Used for moving into the next phase and passing data beforehand.
	/// </summary>
	[SerializeField] private PlayerTurnIdlePhase playerTurnIdlePhase;

	/// <summary>
	/// DEMO ONLY, changes demo music
	/// </summary>
	[SerializeField] private AudioLowPassFilter lowPass;

	/// <summary>
	/// The cat selected to move. Should probably be an actual cat implementation, but we don't have that yet.
	/// </summary>
	public GameCharacter selectedCat;


	public StretchThing stretch;

	override public void OnTakeControl () {
		stretch.transform.position = selectedCat.myTile.characterConnectionPoint;
		stretch.target = selectedCat.myTile.characterConnectionPoint;
		stretch.ImmediateHide ();
		stretch.gameObject.SetActive (true);
		tilePath = new List<Tile> ();
		lowPass.cutoffFrequency = 1000f;
	}
		
	// pretty much pure proof of concept here
	override public void ControlUpdate () {
		// draw dat arrow tho

		if (brain.tileManager.cursorTile != brain.tileManager.tileMousedOver) {
			if (brain.tileManager.tileMousedOver == null || brain.tileManager.tileMousedOver.tileType == TileType.Wall || brain.tileManager.tileMousedOver.occupant != null) {
				brain.tileManager.cursorTile = null;
				stretch.target = selectedCat.myTile.characterConnectionPoint;
			}
			else {
				brain.tileManager.cursorTile = brain.tileManager.tileMousedOver;
				stretch.target = brain.tileManager.tileMousedOver.characterConnectionPoint;
			}
		}

		if (Input.GetMouseButton (0) == false) {
			// NORMALLY transition to the arrow menu phase

			// DEMO
			selectedCat.MoveTo (brain.tileManager.tileMousedOver);
			brain.tileManager.cursorTile = selectedCat.myTile;

			playerTurnIdlePhase.TakeControl ();
		}
	}

	override public void OnLeaveControl () {
		stretch.gameObject.SetActive (false);
		selectedCat = null;
		lowPass.cutoffFrequency = 22000f;
	}
}
