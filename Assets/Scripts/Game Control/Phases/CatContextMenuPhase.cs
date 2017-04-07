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
	/// The arrow segment parent. Will be destroyed.
	/// </summary>
	[HideInInspector] public GameObject arrowSegmentParent;

	/// <summary>
	/// The ordered path the cat will take.
	/// </summary>
	[HideInInspector] public List<Tile> tilePath;

	override public void OnTakeControl () {
		//UIManager.CenterPathEndMenuOnMouse ();
		Tile last = tilePath.LastElement ();
		UIManager.CenterPathEndMenuOnWorldPoint (last.topCenterPoint);
		UIManager.pathEndMenuState = true;
		last.shimmer = true;
		TileManager.cursorTile = last;

		CameraOverheadControl.dragControlAllowed = true;
	}

	public override void UIRestButtonEvent () {
		catExecutePhase.selectedCat = selectedCat;
		catExecutePhase.tilePath = tilePath;
		catExecutePhase.TakeControl ();
	}

	override public void UICancelPathButtonEvent () {
		playerTurnIdlePhase.TakeControl ();
	}

	override public void OnLeaveControl () {
		UIManager.pathEndMenuState = false;
		TileManager.ClearAllShimmer ();
		TileManager.cursorTile = null;
		GameObject.Destroy (arrowSegmentParent);
		CameraOverheadControl.dragControlAllowed = false;
	}
}
