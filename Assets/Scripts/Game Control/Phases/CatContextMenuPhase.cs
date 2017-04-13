using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// During this phase, the player has already selected a path for a certain cat to move. This phase waits for the player to decide to rest, cancel, or use an action.
/// Exits: PlayerTurnIdlePhase, CatExecutePhase, action targeting (not implemented)
/// </summary>
public class CatContextMenuPhase : GameControlPhase {
	/// <summary>
	/// Used for static TakeControl
	/// </summary>
	private static CatContextMenuPhase staticInstance;
	/// <summary>
	/// Puts the CatContextMenuPhase in control
	/// </summary>
	public static void TakeControl (Cat selectedCat, List<Tile> tilePath) {
		staticInstance.selectedCat = selectedCat;
		staticInstance.tilePath = tilePath;
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}

	/// <summary>
	/// Target cat.
	/// </summary>
	private Cat selectedCat;


	/// <summary>
	/// The ordered path the cat will take.
	/// </summary>
	private List<Tile> tilePath;

	override public void OnTakeControl () {
		TileManager.ClearAllShimmer ();
		Tile last = tilePath.LastElement ();
		UIManager.CenterPathEndMenuOnWorldPoint (last.topCenterPoint);
		UIManager.pathEndMenuState = true;
		last.shimmer = true;
		TileManager.cursorTile = last;

		CameraOverheadControl.dragControlAllowed = true;
	}

	public override void UIRestButtonEvent () {
		TileManager.ClearAllShimmer ();
		TileManager.cursorTile = null;

		CameraOverheadControl.dragControlAllowed = false;

		CatExecutePhase.TakeControl (selectedCat, tilePath);
	}

	override public void UICancelPathButtonEvent () {
		PlayerTurnIdlePhase.SelectTile (selectedCat.myTile);
		PlayerTurnIdlePhase.TakeControl ();
	}

	override public void OnLeaveControl () {
		DrawArrowPhase.ClearArrow ();
		UIManager.pathEndMenuState = false;
	}
}
