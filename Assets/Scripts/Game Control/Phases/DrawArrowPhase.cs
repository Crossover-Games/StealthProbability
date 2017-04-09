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
	/// Pool of objects used to draw the arrow.
	/// </summary>
	public static GameObject[] lineSegments;
	[SerializeField] private GameObject[] m_lineSegments;

	/// <summary>
	/// The cat selected to move.
	/// </summary>
	[HideInInspector] public Cat selectedCat;

	private class TileAndLine {
		public TileAndLine (Tile t, GameObject go) {
			tile = t;
			line = go;
		}
		public Tile tile;
		public GameObject line;
	}

	void Awake () {
		lineSegments = m_lineSegments;
	}

	private List<TileAndLine> tileLinePath = new List<TileAndLine> ();
	private HashSet<Tile> availableTiles = new HashSet<Tile> ();
	private Tile endOfPath {
		get {
			if (tileLinePath.Count == 0) {
				return null;
			}
			else {
				return tileLinePath.LastElement ().tile;
			}
		}
	}

	private void UpdateAvailableTiles () {
		HashSet<Tile> previouslyHighlighted = new HashSet<Tile> (availableTiles);

		availableTiles = new HashSet<Tile> ();
		availableTiles.Add (endOfPath);

		for (int x = 0; x < selectedCat.maxEnergy + 1 - tileLinePath.Count; x++) {
			HashSet<Tile> tempTiles = new HashSet<Tile> (availableTiles);
			foreach (Tile t in tempTiles) {
				foreach (Compass.Direction d in Compass.allDirections) {
					Tile tmp = t.GetNeighborInDirection (d);
					if (Tile.ValidStepDestination (tmp) && !tileLinePath.Exists ((TileAndLine test) => test.tile == tmp)) {
						availableTiles.Add (tmp);
					}
				}
			}
		}
		availableTiles.Remove (endOfPath);
		TileManager.MassSetShimmer (availableTiles);
	}

	override public void OnTakeControl () {
		tileLinePath = new List<TileAndLine> ();
		tileLinePath.Add (new TileAndLine (selectedCat.myTile, null));

		UpdateAvailableTiles ();
	}

	override public void MouseOverChangeEvent () {
		// if the moused over tile is a valid step in the path
		if (tileLinePath.Count <= selectedCat.maxEnergy && availableTiles.Contains (TileManager.tileMousedOver) && tileLinePath.LastElement ().tile.IsNeighbor (TileManager.tileMousedOver)) {
			AddTileToPath (TileManager.tileMousedOver);
		}
	}

	/// <summary>
	/// If mouse is not held, go to next phase. If the mouse changes, update the path.
	/// </summary>
	override public void ControlUpdate () {
		if (Input.GetMouseButton (0) == false) {
			TileManager.cursorTile = null;

			List<Tile> path = new List<Tile> ();
			for (int x = 0; x < tileLinePath.Count; x++) {
				if (x != 0) {
					tileLinePath[x].line.SetActive (false);
				}
				path.Add (tileLinePath[x].tile);
			}

			catContextMenuPhase.tilePath = path;
			catContextMenuPhase.selectedCat = selectedCat;
			catContextMenuPhase.TakeControl ();
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
			else if (tileLinePath.Exists ((TileAndLine tal) => tal.tile == TileManager.tileMousedOver)) {
				int iterations = tileLinePath.Count - 1 - tileLinePath.FindLastIndex ((TileAndLine tal) => tal.tile == TileManager.tileMousedOver);
				for (int x = 0; x < iterations; x++) {
					int lastIndex = tileLinePath.Count - 1;
					tileLinePath[lastIndex].line.SetActive (false);
					tileLinePath.RemoveAt (lastIndex);
					UpdateAvailableTiles ();
				}
			}
		}
	}

	override public void OnLeaveControl () {
		selectedCat = null;
		TileManager.ClearAllShimmer ();
	}

	/// <summary>
	/// Adds the tile to path and updates selectable tiles.
	/// </summary>
	private void AddTileToPath (Tile t) {
		GameObject currentSegment = lineSegments[tileLinePath.Count - 1];
		currentSegment.SetActive (true);

		currentSegment.transform.position = endOfPath.topCenterPoint.HalfwayTo (t.topCenterPoint);
		currentSegment.transform.rotation = Compass.DirectionToRotation (endOfPath.GetDirectionOfNeighbor (t));
		tileLinePath.Add (new TileAndLine (t, currentSegment));

		UpdateAvailableTiles ();
	}
}
