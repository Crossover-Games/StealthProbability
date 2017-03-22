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

	private bool catsAvailable = true;
	/// <summary>
	/// All cats the player can control.
	/// </summary>
	public List<Cat> allCats;

	[Tooltip ("Parent of all cats in the scene.")]
	[SerializeField] private GameObject catParent;

	void Awake () {
		allCats = new List<Cat> (catParent.GetComponentsInChildren<Cat> ());
	}

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
				if (t.occupant.characterType == CharacterType.Cat) {
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
						//while(next
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
		if (brain.tileManager.cursorTile.occupant != null && brain.tileManager.cursorTile.occupant.characterType == CharacterType.Cat) {
			ExitToDrawArrowPhase ();
		}
	}

	/// <summary>
	/// Reminds which cats are available to be moved.
	/// </summary>
	override public void OnTakeControl () {	// should update the loose camera follow target to some custom point you control
		CheckCatsAvailable ();
	}

	override public void OnLeaveControl () {
		RemoveShimmer ();
	}

	/// <summary>
	/// Switches to the dog turn if all cats did their move
	/// </summary>
	override public void ControlUpdate () {
		if (!catsAvailable) {
			dogTurnPhase.TakeControl ();
		}
	}

	private void ExitToDrawArrowPhase () {
		drawArrowPhase.selectedCat = brain.tileManager.cursorTile.occupant as Cat;
		drawArrowPhase.TakeControl ();
	}

	private void CheckCatsAvailable () {
		catsAvailable = false;
		foreach (Cat c in allCats) {
			if (c.ableToMove) {
				catsAvailable = true;
			}
		}
	}

	/// <summary>
	/// Un-grays all cats and allows them to move.
	/// </summary>
	public void RejuvenateAllCats () {
		foreach (Cat c in allCats) {
			c.ableToMove = true;
			c.isGrayedOut = false;
		}
		catsAvailable = true;
	}

	private void RemoveShimmer () {
		foreach (Tile t in shimmerInfo) {
			t.shimmer = false;
		}
		shimmerInfo = new List<Tile> ();
	}
}
