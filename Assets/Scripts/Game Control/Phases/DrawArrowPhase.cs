using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game phase for when you're drawing the arrow to move a cat. Leads only into the CatContextMenuPhase.
/// </summary>
public class DrawArrowPhase : GameControlPhase {

	/// <summary>
	/// Exit node to cat context menu phase. Needs to know the target cat and the path.
	/// </summary>
	[SerializeField] private CatContextMenuPhase catContextMenuPhase;

	/// <summary>
	/// The ordered path of all tiles for which the arrow follows.
	/// </summary>
	private List<Floor> tilePath;

	/// <summary>
	/// The cat selected to move.
	/// </summary>
	[HideInInspector] public Cat selectedCat;

	[SerializeField] private GameObject arrowSegment;
	private GameObject arrowSegmentParent;

	private HashSet<Floor> availableTiles = new HashSet<Floor> ();
	private void UpdateAvailableTiles () {
		HashSet<Floor> previouslyHighlighted = new HashSet<Floor> (availableTiles);

		availableTiles = new HashSet<Floor> ();
		availableTiles.Add (tilePath.LastElement ());

		for (int x = 0; x < selectedCat.maxEnergy + 1 - tilePath.Count; x++) {
			HashSet<Floor> tempTiles = new HashSet<Floor> (availableTiles);
			foreach (Floor t in tempTiles) {
				foreach (Compass.Direction d in Compass.allDirections) {
					Floor tmp = t.GetNeighborInDirection (d) as Floor;
					if (Tile.IsValidMoveDestination (tmp) && !tilePath.Contains (tmp)) {
						availableTiles.Add (tmp);
					}
				}
			}
		}

		availableTiles.Remove (tilePath.LastElement ());

		Floor.MassSetShimmer (availableTiles);
	}
		
	override public void OnTakeControl () {
		arrowSegmentParent = new GameObject ();
		//UniversalTileManager.cursorTile = null;

		tilePath = new List<Floor> ();
		tilePath.Add (selectedCat.myTile);

		UpdateAvailableTiles ();
	}

	override public void MouseOverChangeEvent () {
		// if the moused over tile is a valid step in the path
		if (tilePath.Count <= selectedCat.maxEnergy && availableTiles.Contains (TileManager.floorTileMousedOver) && tilePath.LastElement ().IsNeighbor (TileManager.tileMousedOver)) {
			AddTileToPath (TileManager.floorTileMousedOver);
		}
	}
		
	/// <summary>
	/// If mouse is not held, go to next phase. If the mouse changes, update the path.
	/// </summary>
	override public void ControlUpdate () {
		if (Input.GetMouseButton (0) == false) {
			TileManager.cursorTile = null;

			catContextMenuPhase.tilePath = tilePath;
			catContextMenuPhase.selectedCat = selectedCat;
			catContextMenuPhase.arrowSegmentParent = arrowSegmentParent;
			catContextMenuPhase.TakeControl ();
		}
		else if (TileManager.tileMousedOver != tilePath.LastElement () && availableTiles.Contains (TileManager.floorTileMousedOver)) {
			// simple solution: don't run the shortest path algorithm
			if (TileManager.tileMousedOver.IsNeighbor (tilePath.LastElement ())) {
				AddTileToPath (TileManager.floorTileMousedOver);
			}
			else {
				List<Floor> pathMap = availableTiles.ToList ();
				Floor lastVisited = tilePath.LastElement ();
				pathMap.Add (lastVisited);
				List<Floor> newPath = Pathfinding.ShortestPath (lastVisited, TileManager.floorTileMousedOver, pathMap);
				newPath = newPath.Subset (1);
				foreach (Floor t in newPath) {
					AddTileToPath (t);
				}
			}
		}
	}

	override public void OnLeaveControl () {
		selectedCat = null;
		Floor.ClearAllShimmer ();
	}

	/// <summary>
	/// Adds the tile to path and updates selectable tiles.
	/// </summary>
	private void AddTileToPath (Floor t) {
		//tilePath.LastElement ().characterConnectionPoint.Halfway (t.characterConnectionPoint);
		Floor head = tilePath.LastElement ();
		GameObject tmp = GameObject.Instantiate (arrowSegment, arrowSegmentParent.transform);
		tmp.transform.position = head.topCenterPoint.Halfway (t.topCenterPoint);
		tmp.transform.rotation = Compass.DirectionToRotation (head.GetDirectionOfNeighbor (t));
		tilePath.Add (t);
		UpdateAvailableTiles ();
	}
}
