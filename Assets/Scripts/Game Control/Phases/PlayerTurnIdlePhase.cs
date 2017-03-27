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

	/// <summary>
	/// Moves cursor
	/// </summary>
	override public void TileClickEvent (Tile t) {
		if (brain.tileManager.cursorTile != t) {
			brain.tileManager.cursorTile = t;


			if (t.occupant != null) {
				if (t.occupant.characterType == CharacterType.Cat && !t.occupant.grayedOut) {
					brain.tileManager.MassSetShimmer (t.AllTraversibleTilesInRadius ((t.occupant as Cat).maxEnergy, false));
				}
				else if (t.occupant.characterType == CharacterType.Dog) {
					HashSet<Tile> toShimmer = new HashSet<Tile> ();
					foreach (PathingNode p in t.pathingNode.AllPotentialPathStarts((t.occupant as Dog).lastVisited)) {
						PathingNode last = t.pathingNode;
						PathingNode current = p;
						while (current != null) {
							toShimmer.Add (current.myTile);
							PathingNode tempLast = current;
							current = current.NextOnPath (last);
							last = tempLast;
						}
						brain.tileManager.MassSetShimmer (toShimmer);
					}
				}
			}
		}
		// once we have the holy grail info box working, we can put stuff like that in there too.
	}

	/// <summary>
	/// Move camera to double clicked tile.
	/// </summary>
	override public void TileDoubleClickEvent (Tile t) {
		brain.cameraControl.SetCamFocusPoint (t.topCenterPoint);
	}

	/// <summary>
	/// Switches to the drag arrow phase.
	/// </summary>
	override public void TileDragEvent (Tile t) {
		if (Input.GetMouseButton (0) && brain.tileManager.cursorTile.occupant != null && brain.tileManager.cursorTile.occupant.characterType == CharacterType.Cat && !brain.tileManager.cursorTile.occupant.grayedOut) {
			ExitToDrawArrowPhase ();
		}
	}

	/// <summary>
	/// allows you to control the camera
	/// </summary>
	override public void OnTakeControl () {
		brain.cameraControl.dragControlAllowed = true;
	}

	override public void OnLeaveControl () {
		brain.cameraControl.dragControlAllowed = false;
	}

	/// <summary>
	/// Switches to the dog turn if all cats did their move
	/// </summary>
	override public void ControlUpdate () {
		if (!brain.catManager.anyAvailable) {
			dogTurnPhase.TakeControl ();
		}
	}

	private void ExitToDrawArrowPhase () {
		drawArrowPhase.selectedCat = brain.tileManager.cursorTile.occupant as Cat;
		drawArrowPhase.TakeControl ();
	}
}
