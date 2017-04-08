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
	[SerializeField] private DogSelectorPhase dogSelectorPhase;

	/// <summary>
	/// Moves cursor and displays overlays. Never null
	/// </summary>
	override public void TileClickEvent (Tile t) {
		if (TileManager.cursorTile != t) {	// changed. now we only want it to do something on a floor

			UIManager.masterInfoBox.ClearAllData ();

			if (TileManager.cursorTile == null || TileManager.cursorTile.tileType == TileType.Wall) {
				TileManager.cursorTile = null;
			}
			else {
				TileManager.cursorTile = t;

				if (t.traversable) {
					
					Floor clickedFloor = t as Floor;
					foreach (TileDangerData tdd in clickedFloor.dangerData) {
						UIManager.masterInfoBox.AddDataFromTileDangerData (tdd);
					}

					if (clickedFloor.occupant != null) {
						UIManager.masterInfoBox.headerText = clickedFloor.occupant.name;
						if (clickedFloor.occupant.characterType == CharacterType.Cat && !clickedFloor.occupant.grayedOut) {
							clickedFloor.AllTilesInRadius ((clickedFloor.occupant as Cat).maxEnergy, true, false);
							Floor.MassSetShimmer (null);
						}
						else if (clickedFloor.occupant.characterType == CharacterType.Dog) {
							HashSet<Floor> toShimmer = new HashSet<Floor> ();
							foreach (PathingNode p in clickedFloor.pathingNode.AllPotentialPathStarts((clickedFloor.occupant as Dog))) {
								PathingNode last = clickedFloor.pathingNode;
								PathingNode current = p;
								while (current != null) {
									toShimmer.Add (current.myTile);
									PathingNode tempLast = current;
									current = current.NextOnPath (last);
									last = tempLast;
								}
								Floor.MassSetShimmer (toShimmer);
							}
						}
					}
					else {
						Floor.ClearAllShimmer ();
						UIManager.masterInfoBox.headerText = "-FLOOR-";
					}
				}
			}
		}




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
		if (Input.GetMouseButton (0) && TileManager.characterUnderCursor != null && TileManager.characterUnderCursor.characterType == CharacterType.Cat && !TileManager.characterUnderCursor.grayedOut) {
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
		drawArrowPhase.selectedCat = (TileManager.cursorTile as Floor).occupant as Cat;
		drawArrowPhase.TakeControl ();
	}
}
