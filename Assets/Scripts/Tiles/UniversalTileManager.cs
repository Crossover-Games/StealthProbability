using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages mouse interaction for tiles and controls the pointer cursor.
/// </summary>
public class UniversalTileManager : MonoBehaviour {

	[SerializeField] private AudioSource dogSound;
	[SerializeField] private AudioSource catSound;

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
			if (value == null) {
				arrowCursor.SetActive (false);
			}
			else {
				if (cursored != value) {
					arrowCursor.transform.position = value.cursorConnectionPoint;
					arrowCursor.SetActive (true);

					// lel factor
					if (value.occupant != null) {
						if (value.occupant.characterType == CharacterType.Cat) {
							catSound.Play ();
						}
						else if (value.occupant.characterType == CharacterType.Dog) {
							dogSound.Play ();
						}
					}
				}
			}
			cursored = value;
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
		
	private float DOUBLE_CLICK_WINDOW = 0.5F;
	private float doubleClickTimeElapsed = 0f;
	private Tile lastClicked = null;

	void Update () {
		if (mousedOver != prevMousedOver) {
			brain.NotifyBrainMouseOverChangeEvent ();
		}
		if (mousedOver != null && Input.GetMouseButtonDown (0)) {
			if (lastClicked != null && lastClicked == mousedOver) {
				brain.NotifyBrainTileDoubleClickEvent (mousedOver);
			}
			else {
				brain.NotifyBrainTileClickEvent (mousedOver);
			}
			RegisterFirstClick (mousedOver);
		}

		if (lastClicked != null) {
			doubleClickTimeElapsed += Time.deltaTime;
		}
		if (doubleClickTimeElapsed > DOUBLE_CLICK_WINDOW) {
			RegisterFirstClick (null);
		}

		prevMousedOver = mousedOver;
	}

	private void RegisterFirstClick (Tile t) {
		lastClicked = t;
		doubleClickTimeElapsed = 0f;
	}
}
