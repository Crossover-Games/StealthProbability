﻿using System.Collections;
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
	[SerializeField] private DogSelectorPhase dogSelectorPhase;

	/// <summary>
	/// Moves cursor and displays overlays. Never null
	/// </summary>
	override public void TileClickEvent (Tile t) {
		if (TileManager.cursorTile != t) {
			TileManager.cursorTile = t;
			UIManager.masterInfoBox.ClearAllData ();
			foreach (TileDangerData tdd in t.dangerData) {
				UIManager.masterInfoBox.AddDataFromTileDangerData (tdd);
			}

			if (t.occupant != null) {
				UIManager.masterInfoBox.headerText = t.occupant.name;
				if (t.occupant.characterType == CharacterType.Cat && !t.occupant.grayedOut) {
					TileManager.MassSetShimmer (t.AllTilesInRadius ((t.occupant as Cat).maxEnergy, true, false));
				}
				else if (t.occupant.characterType == CharacterType.Dog) {
					HashSet<Tile> toShimmer = new HashSet<Tile> ();
					foreach (PathingNode p in t.pathingNode.AllPotentialPathStarts((t.occupant as Dog))) {
						PathingNode last = t.pathingNode;
						PathingNode current = p;
						while (current != null) {
							toShimmer.Add (current.myTile);
							PathingNode tempLast = current;
							current = current.NextOnPath (last);
							last = tempLast;
						}
						TileManager.MassSetShimmer (toShimmer);
					}
				}
			}
			else {
				TileManager.ClearAllShimmer ();
				UIManager.masterInfoBox.headerText = "-FLOOR-";
			}
		}
		// once we have the holy grail info box working, we can put stuff like that in there too.
	}

	/// <summary>
	/// Move camera to double clicked tile.
	/// </summary>
	override public void TileDoubleClickEvent (Tile t) {
		CameraOverheadControl.SetCamFocusPoint (t.topCenterPoint);
	}

	/// <summary>
	/// Switches to the drag arrow phase.
	/// </summary>
	override public void TileDragEvent (Tile t) {
		if (Input.GetMouseButton (0) && TileManager.cursorTile.occupant != null && TileManager.cursorTile.occupant.characterType == CharacterType.Cat && !TileManager.cursorTile.occupant.grayedOut) {
			ExitToDrawArrowPhase ();
		}
	}

	/// <summary>
	/// allows you to control the camera
	/// </summary>
	override public void OnTakeControl () {
		CameraOverheadControl.dragControlAllowed = true;
	}

	override public void OnLeaveControl () {
		UIManager.masterInfoBox.ClearAllData ();		// maybe not
		CameraOverheadControl.dragControlAllowed = false;
	}

	/// <summary>
	/// Switches to the dog turn if all cats did their move
	/// </summary>
	override public void ControlUpdate () {
		if (!GameBrain.catManager.anyAvailable) {
			dogSelectorPhase.TakeControl ();
		}
	}

	private void ExitToDrawArrowPhase () {
		drawArrowPhase.selectedCat = TileManager.cursorTile.occupant as Cat;
		drawArrowPhase.TakeControl ();
	}
}
