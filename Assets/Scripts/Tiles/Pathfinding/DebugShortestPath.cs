using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When you right click: finds the shortest path from the cursor tile to the moused over tile.
/// </summary>
public class DebugShortestPath : MonoBehaviour {

	private GameBrain brain;

	void Awake () {
		brain = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameBrain> ();
	}

	void Update () {
		if (Input.GetMouseButtonDown (1) && TileManager.cursorTile != null && TileManager.tileMousedOver != null) {
			TileManager.tileMousedOver.shimmer = true;

			List<Tile> validTiles = new List<Tile> ();
			foreach (Tile t in TileManager.allTiles) {
				if (t.IsValidMoveDestination) {
					validTiles.Add (t);
				}
			}
				
			List<Tile> path = Pathfinding.ShortestPath (TileManager.cursorTile, TileManager.tileMousedOver, validTiles);
			for (int x = 0; x < path.Count; x++) {
				path [x].shimmer = true;
			}
		}
	}
}
