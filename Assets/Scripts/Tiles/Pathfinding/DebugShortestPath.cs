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
		if (Input.GetMouseButtonDown (1) && brain.tileManager.cursorTile != null && brain.tileManager.tileMousedOver != null) {
			brain.tileManager.tileMousedOver.dangerColor = Color.red;
			brain.tileManager.tileMousedOver.dangerVisualizerEnabled = true;

			List<Tile> validTiles = new List<Tile> ();
			foreach (Tile t in brain.tileManager.allTiles) {
				if (t.IsValidMoveDestination) {
					validTiles.Add (t);
				}
			}
				
			List<Tile> path = Pathfinding.ShortestPath (brain.tileManager.cursorTile, brain.tileManager.tileMousedOver, validTiles);
			for (int x = 0; x < path.Count; x++) {
				path [x].dangerColor = Color.Lerp (Color.green, Color.white, (1f * x) / (path.Count - 1));
				path [x].dangerVisualizerEnabled = true;
			}
		}
	}
}
