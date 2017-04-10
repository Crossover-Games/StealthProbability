using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This phase is when you're just looking around the map, planning out your turn. Leads into the DrawArrowPhase and dog turn.
/// Exits: DrawArrowPhase, DogSelectorPhase
/// </summary>
public class PlayerTurnIdlePhase : GameControlPhase {
	/// <summary>
	/// Used for static TakeControl
	/// </summary>
	private static PlayerTurnIdlePhase staticInstance;
	/// <summary>
	/// Puts the PlayerTurnIdlePhase in control
	/// </summary>
	public static void TakeControl () {
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}

	/// <summary>
	/// Moves the cursor, updates UI and highlights. Does not trigger click events.
	/// </summary>
	/// <param name="t"></param>
	public static void SelectTile (Tile t) {
		staticInstance.UpdateAfterClick (t);
	}

	/// <summary>
	/// Moves the cursor, updates UI and highlights
	/// </summary>
	private void UpdateAfterClick (Tile t) {
		if (t != null) {
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
					foreach (PathingNode p in t.pathingNode.AllPotentialPathStarts ((t.occupant as Dog))) {
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
	}
	/// <summary>
	/// Moves cursor and displays overlays. Never null
	/// </summary>
	override public void TileClickEvent (Tile t) {
		if (TileManager.cursorTile != t) {	//if different
			if (t.occupant != null) {
				t.occupant.PlaySound ();
			}
			UpdateAfterClick (t);
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
		UIManager.masterInfoBox.ClearAllData ();        // maybe not
		CameraOverheadControl.dragControlAllowed = false;
	}

	/// <summary>
	/// Switches to the dog turn if all cats did their move
	/// </summary>
	override public void ControlUpdate () {
		if (!GameBrain.catManager.anyAvailable) {
			DogSelectorPhase.TakeControl ();
		}
	}

	private void ExitToDrawArrowPhase () {
		DrawArrowPhase.TakeControl (TileManager.cursorTile.occupant as Cat);
	}
}
