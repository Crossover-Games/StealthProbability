using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game phase for when you're drawing the arrow to move a cat. Leads only into the ArrowMenuPhase.
/// </summary>
public class DrawArrowPhase : GameControlPhase {
	// override public void TileClickEvent (Tile t)

	/// <summary>
	/// Exit node to player turn idle phase.
	/// </summary>
	[SerializeField] private PlayerTurnIdlePhase playerTurnIdlePhase;

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
	private void HighlightAvailableSquares () {
		HashSet<Tile> previouslyHighlighted  = new HashSet<Tile> (availableTiles);

		availableTiles = new HashSet<Tile> ();
		availableTiles.Add (tilePath.LastElement ());

		for (int x = 0; x < selectedCat.maxEnergy + 1 - tilePath.Count; x++) {
			HashSet<Tile> tempTiles = new HashSet<Tile> (availableTiles);
			foreach (Tile t in tempTiles) {
				foreach (Compass.Direction d in Compass.allDirections) {
					Tile tmp = t.GetNeighborInDirection (d);
					if (tmp != null && tmp.tileType != TileType.Wall && tmp.occupant == null) {
						availableTiles.Add (tmp);
					}
				}
			}
		}

		foreach (Tile t in tilePath) {
			availableTiles.Remove (t);
		}

		foreach (Tile t in previouslyHighlighted) {
			if (!availableTiles.Contains (t)) {
				t.dangerVisualizerEnabled = false;
			}
		}

		foreach (Tile t in availableTiles) {
			t.dangerColor = DangerSquareVisualizer.CAROLINA_BLUE;
			t.dangerVisualizerEnabled = true;
		}
	}


	override public void OnTakeControl () {
		arrowSegmentParent = new GameObject ();

		tilePath = new List<Tile> ();
		tilePath.Add (selectedCat.myTile);
		HighlightAvailableSquares ();

		lowPass.cutoffFrequency = 1000f;
	}

	override public void MouseOverChangeEvent () {
		// if the moused over tile is a valid step in the path
		if (tilePath.Count <= selectedCat.maxEnergy && availableTiles.Contains (brain.tileManager.tileMousedOver) && tilePath.LastElement ().IsNeighbor (brain.tileManager.tileMousedOver)) {
			AddTileToPath (brain.tileManager.tileMousedOver);
			HighlightAvailableSquares ();
		}
	}
		
	override public void ControlUpdate () {
		// if mouse is released, change phases.
		if (Input.GetMouseButton (0) == false) {
			brain.tileManager.cursorTile = null;

			catContextMenuPhase.tilePath = tilePath;
			catContextMenuPhase.selectedCat = selectedCat;
			catContextMenuPhase.TakeControl ();
		}
	}

	override public void OnLeaveControl () {
		selectedCat = null;
		GameObject.Destroy (arrowSegmentParent);

		foreach (Tile t in availableTiles) {
			t.dangerVisualizerEnabled = false;
		}

		lowPass.cutoffFrequency = 22000f;
	}

	private void AddTileToPath (Tile t) {
		//tilePath.LastElement ().characterConnectionPoint.Halfway (t.characterConnectionPoint);
		Tile head = tilePath.LastElement ();
		GameObject tmp = GameObject.Instantiate (arrowSegment, arrowSegmentParent.transform);
		tmp.transform.position = head.characterConnectionPoint.Halfway (t.characterConnectionPoint);
		tmp.transform.rotation = Compass.DirectionToRotation (head.GetDirectionOfNeighbor (t));
		tilePath.Add (t);
	}
}
