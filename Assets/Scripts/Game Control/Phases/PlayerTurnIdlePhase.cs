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
		staticInstance.lastCatClicked = null;
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}

	private Cat lastCatClicked;

	/// <summary>
	/// Moves the cursor, updates UI and highlights. Does not trigger click events.
	/// </summary>
	public static void SelectTile (Tile t) {
		staticInstance.UpdateAfterClick (t);
	}

	/// <summary>
	/// Moves the cursor, updates UI and highlights
	/// </summary>
	private void UpdateAfterClick (Tile t) {
		TileManager.cursorTile = t;
		UIManager.masterInfoBox.ClearAllData ();
		if (t == null) {
			UIManager.masterInfoBox.headerText = "";
			TileManager.ClearAllShimmer ();
			UIManager.routeCurrentlyDisplayed = null;
		}
		else if (t != null) {
			if (t.occupant != null) {
				UIManager.masterInfoBox.AddData (t.occupant.flavorText, t.occupant.defaultColor.OptimizedForText ());
				UIManager.masterInfoBox.headerText = t.occupant.name;
				if (t.occupant.characterType == CharacterType.Cat) {
					Cat thisCat = (t.occupant as Cat);
					lastCatClicked = thisCat;
					if (thisCat.isWet) {
						string soakText = "SOAKED! Dry after " + thisCat.wetTurnsRemaining + " turn";
						if (thisCat.wetTurnsRemaining != 1) {
							soakText += "s";
						}
						UIManager.masterInfoBox.AddData (soakText, WetFloor.waterColor);
					}
					if (thisCat.hasWildCard) {
						UIManager.masterInfoBox.AddData ("Second chance ready", Color.white);
					}
					else {
						UIManager.masterInfoBox.AddData ("Second chance depleted", Color.gray);
					}

					if (thisCat.stealthStacks > 0) {
						UIManager.masterInfoBox.AddData (thisCat.stealthStacks.ToString () + " energy converted " + thisCat.stealthStacks.ToString () + "0% danger reduction", Color.cyan);
					}

					if (!t.occupant.grayedOut) {
						TileManager.MassSetShimmer (t.AllTilesInRadius (thisCat.maxEnergy, true, false));
						UIManager.masterInfoBox.AddEnergyDataFromCat (thisCat.maxEnergy, thisCat);
					}
					else {
						TileManager.ClearAllShimmer ();
						UIManager.masterInfoBox.AddEnergyDataFromCat (0, thisCat);
					}
					UIManager.routeCurrentlyDisplayed = null;
				}
				else if (t.occupant.characterType == CharacterType.Dog) {
					TileManager.ClearAllShimmer ();
					foreach (TileDangerData tdd in (t.occupant as Dog).visionPattern.allTilesAffected) {
						tdd.myTile.shimmer = true;
					}
					UIManager.routeCurrentlyDisplayed = (t.occupant as Dog).route;
				}
				else if (t.occupant.characterType == CharacterType.Machine) {
					TileManager.ClearAllShimmer ();
					foreach (TileDangerData tdd in (t.occupant as Dog).visionPattern.allTilesAffected) {
						tdd.myTile.shimmer = true;
					}
				}
			}
			else {
				TileManager.ClearAllShimmer ();
				foreach (TileDangerData tdd in t.dangerData) {
					UIManager.masterInfoBox.AddDataFromTileDangerData (tdd);
					tdd.watchingDog.myTile.shimmer = true;
				}
				UIManager.masterInfoBox.headerText = t.tileName;
				UIManager.masterInfoBox.AddDataFromTile (t);
				UIManager.routeCurrentlyDisplayed = null;
			}
		}
	}

	/// <summary>
	/// Moves cursor and displays overlays. Can be null.
	/// </summary>
	private void CustomTileClickEvent (Tile t) {
		if (TileManager.cursorTile != t) {
			if (t == null) {
				UpdateAfterClick (t);
			}
			else if (t.occupant != null) {
				t.occupant.PlaySound ();
				UpdateAfterClick (t);
			}
			else {
				if (TileManager.cursorTile != null && TileManager.cursorTile.occupant != null && TileManager.cursorTile.occupant.characterType == CharacterType.Cat && t.shimmer) {
					DrawArrowPhase.TakeControlAutoPath (TileManager.cursorTile.occupant as Cat, t);
				}
				else {
					UpdateAfterClick (t);
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
		if (Input.GetMouseButton (0) && TileManager.cursorTile != null && TileManager.cursorTile.occupant != null && TileManager.cursorTile.occupant.characterType == CharacterType.Cat && !TileManager.cursorTile.occupant.grayedOut) {
			DrawArrowPhase.TakeControl (TileManager.cursorTile.occupant as Cat);
		}
	}

	/// <summary>
	/// allows you to control the camera
	/// </summary>
	override public void OnTakeControl () {
		CameraOverheadControl.dragControlAllowed = true;
	}

	override public void UIStayButtonEvent () {
		TileManager.ClearAllShimmer ();
		TileManager.cursorTile = null;
		CatExecutePhase.TakeControl (lastCatClicked, new List<Tile> ());
	}

	override public void OnLeaveControl () {
		CameraOverheadControl.dragControlAllowed = false;
		UIManager.stayMenuState = false;
	}

	/// <summary>
	/// Switches to the dog turn if all cats did their move. Also fakes the tile click event.
	/// </summary>
	override public void ControlUpdate () {
		if (!GameBrain.catManager.anyAvailable) {
			DogSelectorPhase.TakeControl ();
		}
		else if (Input.GetMouseButtonDown (0)) {
			if (UIManager.stayMenuState) {
				if (!UIManager.mouseOverStayMenu) {
					UIManager.stayMenuState = false;
					CustomTileClickEvent (TileManager.tileMousedOver);
				}
			}
			else {
				CustomTileClickEvent (TileManager.tileMousedOver);
			}
		}
		else if (Input.GetMouseButtonUp (0) && TileManager.tileMousedOver != null && TileManager.tileMousedOver.occupant != null && TileManager.tileMousedOver.occupant is Cat && !TileManager.tileMousedOver.occupant.grayedOut) {
			UIManager.CenterMenusOnWorldPoint (TileManager.tileMousedOver.topCenterPoint);
			UIManager.stayMenuState = true;
		}
	}
}
