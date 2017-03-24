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
	/// All tiles where the selected object may move to
	/// </summary>
	private List<Tile> shimmerInfo = new List<Tile> ();

	/// <summary>
	/// Moves cursor
	/// </summary>
	override public void TileClickEvent (Tile t) {
		if (brain.tileManager.cursorTile != t) {
			brain.tileManager.cursorTile = t;
			RemoveShimmer ();

			if (t.occupant != null) {
				if (t.occupant.characterType == CharacterType.Cat && !t.occupant.grayedOut) {
					shimmerInfo.Add (t);
					for (int x = 0; x < (t.occupant as Cat).maxEnergy; x++) {
						List<Tile> tempShimmer = shimmerInfo.Clone ();
						foreach (Tile tmp in tempShimmer) {
							foreach (Tile neighbor in tmp.allNeighbors) {
								if (UniversalTileManager.IsValidMoveDestination (neighbor)) {
									neighbor.shimmer = true;
									shimmerInfo.Add (neighbor);
								}
							}
						}
					}
				}
				else if (t.occupant.characterType == CharacterType.Dog) {
					foreach (PathingNode p in t.pathingNode.AllPotentialPathStarts((t.occupant as Dog).lastVisited)) {
						PathingNode last = t.pathingNode;
						PathingNode current = p;
						while (current != null) {
							current.myTile.shimmer = true;
							shimmerInfo.Add (current.myTile);
							PathingNode tempLast = current;
							current = current.NextOnPath (last);
							last = tempLast;
						}
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
		RemoveShimmer ();
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

	private void RemoveShimmer () {
		foreach (Tile t in shimmerInfo) {
			t.shimmer = false;
		}
		shimmerInfo = new List<Tile> ();
	}
}
