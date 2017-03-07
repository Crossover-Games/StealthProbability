using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages mouse interaction for tiles and controls the pointer cursor.
/// </summary>
public class UniversalTileManager : MonoBehaviour {

	private Tile prevMousedOver = null;
	private Tile mousedOver = null;
	/// <summary>
	/// The tile the mouse cursor is currently hovering over.
	/// </summary>
	public Tile tileMousedOver {
		get{ return mousedOver; }
	}


	private HashSet<Tile> s_allTiles = new HashSet<Tile> ();
	private Tile[] a_allTiles;
	public Tile[] allTiles {
		get{ return a_allTiles; }
	}

	/// <summary>
	/// Reference to the scene's GameBrain.
	/// </summary>
	[SerializeField] private GameBrain brain;

	/// <summary>
	/// Reference to cursor with tile. Please do not modify directly.
	/// </summary>
	private Tile cursored = null;

	/// <summary>
	/// The game object for the arrow cursor.
	/// </summary>
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

	/// <summary>
	/// Only to be called in Tile.OnMouseEnter().
	/// </summary>
	public void RegisterMouseEnter (Tile t) {
		mousedOver = t;
		mousedOver.mouseOverVisualState = true;
	}
	/// <summary>
	/// Only to be called in Tile.OnMouseExit(). If you mouse off the map rather than on to another tile, the game needs to know you aren't still lingering on some tile. Though less straightforward, it's faster than checking every update.
	/// </summary>
	public void CheckIfUnregisterIsRequired (Tile t) {
		t.mouseOverVisualState = false;
		if (mousedOver == t) {
			mousedOver = null;
		}
	}

	public void RegisterTileSetup (Tile t) {
		s_allTiles.Add (t);
	}

	void Start () {
		a_allTiles = s_allTiles.ToArray ();
	}
		
	void Update () {
		if (mousedOver != prevMousedOver) {
			brain.NotifyBrainMouseOverChangeEvent ();
		}
		if (mousedOver != null && Input.GetMouseButtonDown (0)) {
			brain.NotifyBrainTileClickEvent (mousedOver);
		}

		prevMousedOver = mousedOver;
	}

}
