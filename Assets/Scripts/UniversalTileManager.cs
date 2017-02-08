using UnityEngine;
using System.Collections.Generic;

public class UniversalTileManager : MonoBehaviour {

	private Tile mousedOver = null;

	/// <summary>
	/// Reference to cursor with tile. Please do not modify directly.
	/// </summary>
	private Tile cursored = null;
	[SerializeField] private GameObject arrowCursor;

	/// <summary>
	/// Gets or sets the tile with the arrow cursor pointing over it. Set it to null to disable it.
	/// </summary>
	public Tile cursorTile {
		get{ return cursored; }
		set { 
			cursored = value;
			if (cursored == null) {
				arrowCursor.SetActive (false);
			}
			else {
				arrowCursor.transform.position = cursored.cursorConnectionPoint;
				arrowCursor.SetActive (true);
			}
		}
	}

	private HashSet<Tile> selectedTiles = new HashSet<Tile> ();

	//Demo
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space) && cursorTile != null) {
			SpreadDemo ();
		}
		else if (Input.GetKeyDown (KeyCode.Escape) && cursorTile != null) {
			UnhighlightAndRemoveSelection ();
		}

		if (mousedOver != null) {
			if (Input.GetMouseButtonDown (0)) {
				if (mousedOver == cursorTile) {
					UnhighlightAndRemoveSelection ();
				}
				else {
					HighlightNeighborsDemo ();
				}
			}
			if (Input.GetMouseButton (1)) {
				SelectTileHelper (mousedOver);
				cursorTile = mousedOver;
				HighlightAllSelected ();
			}
			else if (Input.GetKeyDown (KeyCode.W)) {
				HighlightLineDemo (CardinalDirection.North);
			}
			else if (Input.GetKeyDown (KeyCode.S)) {
				HighlightLineDemo (CardinalDirection.South);
			}
			else if (Input.GetKeyDown (KeyCode.A)) {
				HighlightLineDemo (CardinalDirection.West);
			}
			else if (Input.GetKeyDown (KeyCode.D)) {
				HighlightLineDemo (CardinalDirection.East);
			}
			else if (Input.GetKeyDown (KeyCode.D)) {
				HighlightLineDemo (CardinalDirection.East);
			}
			else if (Input.GetKeyDown (KeyCode.Alpha0)) {
				SelectWithinRangeDemo (mousedOver, 0);
			}
			else if (Input.GetKeyDown (KeyCode.Alpha1)) {
				SelectWithinRangeDemo (mousedOver, 1);
			}
			else if (Input.GetKeyDown (KeyCode.Alpha2)) {
				SelectWithinRangeDemo (mousedOver, 2);
			}
			else if (Input.GetKeyDown (KeyCode.Alpha3)) {
				SelectWithinRangeDemo (mousedOver, 3);
			}
			else if (Input.GetKeyDown (KeyCode.Alpha4)) {
				SelectWithinRangeDemo (mousedOver, 4);
			}
			else if (Input.GetKeyDown (KeyCode.Alpha5)) {
				SelectWithinRangeDemo (mousedOver, 5);
			}
			else if (Input.GetKeyDown (KeyCode.Alpha6)) {
				SelectWithinRangeDemo (mousedOver, 6);
			}
			else if (Input.GetKeyDown (KeyCode.Alpha7)) {
				SelectWithinRangeDemo (mousedOver, 7);
			}
			else if (Input.GetKeyDown (KeyCode.Alpha8)) {
				SelectWithinRangeDemo (mousedOver, 8);
			}
			else if (Input.GetKeyDown (KeyCode.Alpha9)) {
				SelectWithinRangeDemo (mousedOver, 9);
			}
		}
	}

	/// <summary>
	/// Only to be called in TileGridUnitVisualizer.OnMouseEnter().
	/// </summary>
	public void RegisterMouseOver (Tile t) {
		//UnhighlightAndRemoveSelection ();
		mousedOver = t;
	}
	/// <summary>
	/// Only to be called in TileGridUnitVisualizer.OnMouseExit(). If you mouse off the map rather than on to another tile, the game needs to know you aren't still lingering on some tile. Though less straightforward, it's faster than checking every update.
	/// </summary>
	public void CheckIfUnregisterIsRequired (Tile t) {
		if (mousedOver == t) {
			mousedOver = null;
		}
	}

	private void HighlightLineDemo (CardinalDirection d) {
		UnhighlightAndRemoveSelection ();

		cursorTile = mousedOver;

		Tile current = cursorTile;
		do {
			selectedTiles.Add (current);
			current = current.GetTileInDirection (d);
		} while (current != null);
		HighlightAllSelected ();
	}
	private void HighlightNeighborsDemo () {
		UnhighlightAndRemoveSelection ();

		cursorTile = mousedOver;

		HighlightNeighborsHelperDemo (cursorTile);
		HighlightAllSelected ();
	}
	/// <summary>
	/// selects the tiles to be highlighted
	/// </summary>
	private void HighlightNeighborsHelperDemo (Tile t) {
		SelectTileHelper (t);
		SelectTileHelper (t.GetTileInDirection (CardinalDirection.North));
		SelectTileHelper (t.GetTileInDirection (CardinalDirection.South));
		SelectTileHelper (t.GetTileInDirection (CardinalDirection.East));
		SelectTileHelper (t.GetTileInDirection (CardinalDirection.West));
	}


	private void SelectWithinRangeDemo (Tile t, int range) {
		UnhighlightAndRemoveSelection ();

		cursorTile = mousedOver;
		SelectTileHelper (cursorTile);
		for (int c = 0; c < range; c++) {
			SpreadDemo ();
		}
		HighlightAllSelected ();
	}

	private void SpreadDemo () {
		HashSet<Tile> tempClone = new HashSet<Tile> (selectedTiles);	// this is like clone
		foreach (Tile t in tempClone) {
			HighlightNeighborsHelperDemo (t);
		}
		HighlightAllSelected ();
	}

	/// <summary>
	/// True highlights, false unhighlights
	/// </summary>
	private void TileHighlightHelper (Tile t, bool highlight = true) {
		if (t != null) {
			t.highlightState = highlight;
		}
	}

	/// <summary>
	/// Ensures null tiles are not added to the set of selected tiles.
	/// </summary>
	private void SelectTileHelper (Tile t) {
		if (t != null) {
			selectedTiles.Add (t);
		}
	}
		
	private void HighlightAllSelected () {
		foreach (Tile t in selectedTiles) {
			TileHighlightHelper (t, true);
		}
	}

	/// <summary>
	/// Unhighlights all highlighted ground and removes the cursor.
	/// </summary>
	private void UnhighlightAndRemoveSelection () {
		cursorTile = null;
		foreach (Tile t in selectedTiles) {
			TileHighlightHelper (t, false);
		}
		selectedTiles = new HashSet<Tile> ();
	}
}
