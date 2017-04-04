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
		get { return cursored; }
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

	private HashSet<Tile> m_shimmering = new HashSet<Tile> ();
	/// <summary>
	/// A list of all shimmering tiles.
	/// </summary>
	public Tile[] shimmeringTiles {
		get { return m_shimmering.ToArray (); }
	}

	/// <summary>
	/// Only called by Tile. Registers tile t as one of the shimmering tiles.
	/// </summary>
	public void RegisterShimmer (Tile t) {
		m_shimmering.Add (t);
	}

	/// <summary>
	/// Only called by Tile. Removes tile t as one of the shimmering tiles.
	/// </summary>
	public void UnregisterShimmer (Tile t) {
		m_shimmering.Remove (t);
	}

	/// <summary>
	/// Remove the shimmer effect from all tiles.
	/// </summary>
	public void ClearAllShimmer () {
		foreach (Tile t in m_shimmering) {
			t.SetCosmeticShimmer (false);
		}
		m_shimmering = new HashSet<Tile> ();
	}

	/// <summary>
	/// Sets these tiles to be the only ones shimmering. Unshimmers all others.
	/// </summary>
	public void MassSetShimmer (ICollection<Tile> tiles) {
		HashSet<Tile> oldTiles = m_shimmering.Clone ();
		HashSet<Tile> newTiles = new HashSet<Tile> (tiles);

		newTiles.ExceptWith (m_shimmering);
		oldTiles.ExceptWith (tiles);

		foreach (Tile t in newTiles) {
			t.shimmer = true;
		}
		foreach (Tile t in oldTiles) {
			t.shimmer = false;
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
	private Tile doubleClickMemory = null;
	private Tile dragMemory = null;

	/// <summary>
	/// Minimum distance the mouse has to move for a drag to be registered.
	/// </summary>
	private float MIN_DRAG_DISTANCE = 20f;
	/// <summary>
	/// Position of the last mouse click.
	/// </summary>
	private Vector3 mouseClickPos;

	void Update () {
		if (mousedOver != null) {
			// mouse over
			if (mousedOver != prevMousedOver) {
				brain.RaiseMouseOverChangeEvent ();
				prevMousedOver = mousedOver;
			}

			// click and double click
			if (Input.GetMouseButtonDown (0)) {
				if (doubleClickMemory != null && doubleClickMemory == mousedOver) {
					brain.RaiseTileDoubleClickEvent (mousedOver);
				}
				else {
					brain.RaiseTileClickEvent (mousedOver);
					dragMemory = mousedOver;
				}
				RegisterFirstClick (mousedOver);
			}
		}


		if (dragMemory != null && Input.GetMouseButton (0)) {
			if (Vector3.Distance (mouseClickPos, Input.mousePosition) >= MIN_DRAG_DISTANCE) {
				brain.RaiseTileDragEvent (doubleClickMemory);
				dragMemory = null;
			}
		}

		if (doubleClickMemory != null) {
			doubleClickTimeElapsed += Time.deltaTime;
		}
		if (doubleClickTimeElapsed > DOUBLE_CLICK_WINDOW) {
			RegisterFirstClick (null);
		}
	}

	/// <summary>
	/// Registers the first click of a double click, or clears it.
	/// </summary>
	private void RegisterFirstClick (Tile t) {
		doubleClickMemory = t;
		mouseClickPos = Input.mousePosition;
		doubleClickTimeElapsed = 0f;
	}

	// ---STATIC METHODS

	/// <summary>
	/// Checks if a certain tile exists, is not obstructed, and is not a wall. Not related to paths or energy.
	/// </summary>
	public static bool IsValidMoveDestination (Tile tile) {
		return (tile != null && tile.tileType != TileType.Wall && tile.occupant == null);
	}

	/// <summary>
	/// Gets the tile at location in <x,z> float coordinates, or null if none. Remember that the center of each tile is at an integer intersection. This uses physics to calculate and may be inefficient. Consider reworking later.
	/// </summary>
	public static Tile GetTileAtLocation (Vector2 location) {
		RaycastHit hit;
		if (Physics.Raycast (new Vector3 (location.x, -100f, location.y), Vector3.up, out hit)) {
			Tile tileTemp = hit.collider.gameObject.GetComponent<Tile> ();
			if (tileTemp != null) {
				return tileTemp;
			}
			else {
				return null;
			}
		}
		return null;
	}
}
