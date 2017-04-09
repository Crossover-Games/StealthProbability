﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game phase for when you're drawing the arrow to move a cat. Leads only into the CatContextMenuPhase.
/// Exits: CatContextMenuPhase
/// </summary>
public class DrawArrowPhase : GameControlPhase {
	/// <summary>
	/// Used for static TakeControl
	/// </summary>
	private static DrawArrowPhase staticInstance;
	/// <summary>
	/// Puts the DrawArrowPhase in control
	/// </summary>
	public static void TakeControl (Cat selectedCat) {
		staticInstance.selectedCat = selectedCat;
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}

	/// <summary>
	/// Removes the arrow.
	/// </summary>
	public static void ClearArrow () {
		staticInstance.pathArrow.ClearArrow ();
	}

	[SerializeField] private PathArrow pathArrow;

	/// <summary>
	/// The cat selected to move.
	/// </summary>
	[HideInInspector] public Cat selectedCat;

	private List<Tile> tilePath = new List<Tile> ();
	private HashSet<Tile> availableTiles = new HashSet<Tile> ();
	private Tile endOfPath {
		get {
			if (tilePath.Count == 0) {
				return null;
			}
			else {
				return tilePath.LastElement ();
			}
		}
	}

	private void UpdateAvailableTiles () {
		HashSet<Tile> previouslyHighlighted = new HashSet<Tile> (availableTiles);

		availableTiles = new HashSet<Tile> ();
		availableTiles.Add (endOfPath);

		for (int x = 0; x < selectedCat.maxEnergy + 1 - tilePath.Count; x++) {
			HashSet<Tile> tempTiles = new HashSet<Tile> (availableTiles);
			foreach (Tile t in tempTiles) {
				foreach (Compass.Direction d in Compass.allDirections) {
					Tile tmp = t.GetNeighborInDirection (d);
					if (Tile.ValidStepDestination (tmp) && !tilePath.Contains (tmp)) {
						availableTiles.Add (tmp);
					}
				}
			}
		}
		availableTiles.Remove (endOfPath);
		TileManager.MassSetShimmer (availableTiles);
	}

	override public void OnTakeControl () {
		tilePath = new List<Tile> ();
		tilePath.Add (selectedCat.myTile);
		UpdateAvailableTiles ();
	}

	override public void MouseOverChangeEvent () {
		// if the moused over tile is a valid step in the path
		if (tilePath.Count <= selectedCat.maxEnergy && availableTiles.Contains (TileManager.tileMousedOver) && tilePath.LastElement ().IsNeighbor (TileManager.tileMousedOver)) {
			AddTileToPath (TileManager.tileMousedOver);
		}
	}

	/// <summary>
	/// If mouse is not held, go to next phase. If the mouse changes, update the path.
	/// </summary>
	override public void ControlUpdate () {
		if (Input.GetMouseButton (0) == false) {
			if (endOfPath.validMoveEnd) {
				TileManager.cursorTile = null;

				CatContextMenuPhase.TakeControl (selectedCat, tilePath);
			}
			else {
				pathArrow.ClearArrow ();
				PlayerTurnIdlePhase.SelectTile (selectedCat.myTile);
				PlayerTurnIdlePhase.TakeControl ();
			}
		}
		else if (TileManager.tileMousedOver != endOfPath) {
			if (availableTiles.Contains (TileManager.tileMousedOver)) {
				// simple solution: don't run the shortest path algorithm
				if (TileManager.tileMousedOver.IsNeighbor (endOfPath)) {
					AddTileToPath (TileManager.tileMousedOver);
				}
				else {
					List<Tile> pathMap = availableTiles.ToList ();
					Tile lastVisited = endOfPath;
					pathMap.Add (lastVisited);
					List<Tile> newPath = Pathfinding.ShortestPath (lastVisited, TileManager.tileMousedOver, pathMap);
					newPath = newPath.Subset (1);
					foreach (Tile t in newPath) {
						AddTileToPath (t);
					}
				}
			}
			// Wind back the list
			else if (tilePath.Contains (TileManager.tileMousedOver)) {
				int iterations = tilePath.Count - 1 - tilePath.LastIndexOf (TileManager.tileMousedOver);
				for (int x = 0; x < iterations; x++) {
					int lastIndex = tilePath.Count - 1;
					pathArrow.lineSegments [lastIndex - 1].SetActive (false);
					tilePath.RemoveAt (lastIndex);
					UpdateAvailableTiles ();
				}
			}
		}
	}

	/// <summary>
	/// Adds the tile to path and updates selectable tiles.
	/// </summary>
	private void AddTileToPath (Tile t) {
		GameObject currentSegment = pathArrow.lineSegments [tilePath.Count - 1];
		currentSegment.SetActive (true);
		currentSegment.transform.position = endOfPath.topCenterPoint.HalfwayTo (t.topCenterPoint);
		currentSegment.transform.rotation = Compass.DirectionToRotation (endOfPath.GetDirectionOfNeighbor (t));

		tilePath.Add (t);
		UpdateAvailableTiles ();
	}
}
