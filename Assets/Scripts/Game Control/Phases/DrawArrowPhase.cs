using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game phase for when you're drawing the arrow to move a cat. Leads only into the ArrowMenuPhase.
/// </summary>
public class DrawArrowPhase : GameControlPhase {
	// override public void TileClickEvent (Tile t)

	/// <summary>
	/// Exit node to cat context menu phase. Needs to know the target cat and the path.
	/// </summary>
	[SerializeField] private CatContextMenuPhase catContextMenuPhase;

	/// <summary>
	/// DEMO ONLY, changes demo music
	/// </summary>
	[SerializeField] private AudioLowPassFilter lowPass;

	/// <summary>
	/// The ordered path of all tiles for which the arrow follows.
	/// </summary>
	private List<Tile> tilePath;

	/// <summary>
	/// The cat selected to move.
	/// </summary>
	[HideInInspector] public Cat selectedCat;

	[SerializeField] private GameObject arrowSegment;
	private GameObject arrowSegmentParent;

	private HashSet<Tile> availableTiles = new HashSet<Tile> ();
	private void UpdateAvailableTiles () {
		HashSet<Tile> previouslyHighlighted = new HashSet<Tile> (availableTiles);

		availableTiles = new HashSet<Tile> ();
		availableTiles.Add (tilePath.LastElement ());

		for (int x = 0; x < selectedCat.maxEnergy + 1 - tilePath.Count; x++) {
			HashSet<Tile> tempTiles = new HashSet<Tile> (availableTiles);
			foreach (Tile t in tempTiles) {
				foreach (Compass.Direction d in Compass.allDirections) {
					Tile tmp = t.GetNeighborInDirection (d);
					if (!tilePath.Contains (tmp) && UniversalTileManager.IsValidMoveDestination (tmp)) {
						availableTiles.Add (tmp);
					}
				}
			}
		}

		availableTiles.Remove (tilePath.LastElement ());

		foreach (Tile t in previouslyHighlighted) {
			if (!availableTiles.Contains (t)) {
				t.shimmer = false;
			}
		}

		foreach (Tile t in availableTiles) {
			t.shimmer = true;
		}
	}
		
	override public void OnTakeControl () {
		arrowSegmentParent = new GameObject ();

		tilePath = new List<Tile> ();
		tilePath.Add (selectedCat.myTile);

		UpdateAvailableTiles ();

		lowPass.cutoffFrequency = 1000f;
	}

	override public void MouseOverChangeEvent () {
		// if the moused over tile is a valid step in the path
		if (tilePath.Count <= selectedCat.maxEnergy && availableTiles.Contains (brain.tileManager.tileMousedOver) && tilePath.LastElement ().IsNeighbor (brain.tileManager.tileMousedOver)) {
			AddTileToPath (brain.tileManager.tileMousedOver);
		}
	}
		
	/// <summary>
	/// If mouse is not held, go to next phase. If the mouse changes, update the path.
	/// </summary>
	override public void ControlUpdate () {
		if (Input.GetMouseButton (0) == false) {
			brain.tileManager.cursorTile = null;

			catContextMenuPhase.tilePath = tilePath;
			catContextMenuPhase.selectedCat = selectedCat;
			catContextMenuPhase.TakeControl ();
		}
		else if (brain.tileManager.tileMousedOver != tilePath.LastElement () && availableTiles.Contains (brain.tileManager.tileMousedOver)) {
			// simple solution: don't run the shortest path algorithm
			if (brain.tileManager.tileMousedOver.IsNeighbor (tilePath.LastElement ())) {
				AddTileToPath (brain.tileManager.tileMousedOver);
			}
			else {
				List<Tile> pathMap = availableTiles.ToList ();
				Tile lastVisited = tilePath.LastElement ();
				pathMap.Add (lastVisited);
				List<Tile> newPath = Pathfinding.ShortestPath (lastVisited, brain.tileManager.tileMousedOver, pathMap);
				newPath = newPath.Subset (1);
				foreach (Tile t in newPath) {
					AddTileToPath (t);
				}
			}
		}
	}

	override public void OnLeaveControl () {
		selectedCat = null;
		GameObject.Destroy (arrowSegmentParent);

		foreach (Tile t in availableTiles) {
			t.shimmer = false;
		}
	}

	/// <summary>
	/// Adds the tile to path and updates selectable tiles.
	/// </summary>
	private void AddTileToPath (Tile t) {
		//tilePath.LastElement ().characterConnectionPoint.Halfway (t.characterConnectionPoint);
		Tile head = tilePath.LastElement ();
		GameObject tmp = GameObject.Instantiate (arrowSegment, arrowSegmentParent.transform);
		tmp.transform.position = head.topCenterPoint.Halfway (t.topCenterPoint);
		tmp.transform.rotation = Compass.DirectionToRotation (head.GetDirectionOfNeighbor (t));
		tilePath.Add (t);
		UpdateAvailableTiles ();
	}
}
