using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages mouse interaction for tiles and controls the pointer cursor.
/// </summary>
public class TileManager : MonoBehaviour {

	private static Tile prevMousedOver = null;
	private static Tile mousedOver = null;
	/// <summary>
	/// The tile the mouse cursor is currently hovering over.
	/// </summary>
	public static Tile tileMousedOver {
		get{ return mousedOver; }
	}
	/// <summary>
	/// The traversable floor tile the mouse cursor is currently hovering over, if any.
	/// CONSIDER REWORKING WHEN YOU'RE NOT TIRED
	/// </summary>
	public static Floor floorTileMousedOver {
		get {
			if (mousedOver == null) {
				return null;
			}
			else if (!mousedOver.traversable) {
				return null;
			}
			else {
				return mousedOver as Floor;
			}
		}
	}

	/// <summary>
	/// Set of all tiles. Forgotten after start.
	/// </summary>
	private static HashSet<Tile> s_allTiles = new HashSet<Tile> ();
	/// <summary>
	/// Array of all tiles. Encapsulated by allTiles. Exists for time efficiency.
	/// </summary>
	private static Tile[] a_allTiles;

	/// <summary>
	/// All tiles in the level.
	/// </summary>
	public static Tile[] allTiles {
		get{ return a_allTiles; }
	}

	/// <summary>
	/// Reference to cursor with tile. Please do not modify directly. Encapsulated by cursorTile
	/// </summary>
	private static Tile cursored = null;


	[SerializeField] private GameObject arrowCursorInstance;
	/// <summary>
	/// The game object for the arrow cursor.
	/// </summary>
	private static GameObject arrowCursor;

	/// <summary>
	/// Gets or sets the tile with the arrow cursor pointing over it. Set it to null to disable it.
	/// </summary>
	public static Tile cursorTile {
		get { return cursored; }
		set { 
			if (value == null) {
				arrowCursor.SetActive (false);
			}
			else {
				if (cursored != value) {
					arrowCursor.transform.position = value.cursorConnectionPoint;
					arrowCursor.SetActive (true);
				}
			}
			cursored = value;
		}
	}

	/// <summary>
	/// If the cursor is pointing to a tile with a character in it, this is it.
	/// </summary>
	public static GameCharacter characterUnderCursor {
		get {
			if (cursored == null || !cursored.traversable) {
				return null;
			}
			return (cursored as Floor).occupant;
		}
	}

	/// <summary>
	/// Only to be called in TileGridUnitVisualizer.OnMouseEnter().
	/// </summary>
	public static void RegisterMouseEnter (Tile t) {
		mousedOver = t;
		mousedOver.mouseOverVisualState = true;
	}
	/// <summary>
	/// Only to be called in Tile.OnMouseExit(). If you mouse off the map rather than on to another tile, the game needs to know you aren't still lingering on some tile. Though less straightforward, it's faster than checking every update.
	/// </summary>
	public static void CheckIfUnregisterIsRequired (Tile t) {
		t.mouseOverVisualState = false;
		if (mousedOver == t) {
			mousedOver = null;
		}
	}

	/// <summary>
	/// On setup, let the tile manager know that this tile exists. Does nothing if not called in Awake.
	/// </summary>
	public static void RegisterTileSetup (Tile t) {
		s_allTiles.Add (t);
	}

	void Awake () {
		arrowCursor = arrowCursorInstance;
	}

	void Start () {
		a_allTiles = s_allTiles.ToArray ();
	}
		
	private static float DOUBLE_CLICK_WINDOW = 0.5F;
	private static float doubleClickTimeElapsed = 0f;
	private static Tile doubleClickMemory = null;
	private static Tile dragMemory = null;

	/// <summary>
	/// Minimum distance the mouse has to move for a drag to be registered.
	/// </summary>
	private static float MIN_DRAG_DISTANCE = 20f;
	/// <summary>
	/// Position of the last mouse click.
	/// </summary>
	private static Vector3 mouseClickPos;

	/// <summary>
	/// Controls mouse over, click, double click, and drag.
	/// </summary>
	void Update () {
		if (mousedOver != null) {
			// mouse over
			if (mousedOver != prevMousedOver) {
				GameBrain.RaiseMouseOverChangeEvent ();
				prevMousedOver = mousedOver;
			}

			// click and double click
			if (Input.GetMouseButtonDown (0)) {
				if (doubleClickMemory != null && doubleClickMemory == mousedOver) {
					GameBrain.RaiseTileDoubleClickEvent (mousedOver);
				}
				else {
					GameBrain.RaiseTileClickEvent (mousedOver);
					dragMemory = mousedOver;
				}
				RegisterFirstClick (mousedOver);
			}
		}


		if (dragMemory != null && Input.GetMouseButton (0)) {
			if (Vector3.Distance (mouseClickPos, Input.mousePosition) >= MIN_DRAG_DISTANCE) {
				GameBrain.RaiseTileDragEvent (doubleClickMemory);
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
	private static void RegisterFirstClick (Tile t) {
		doubleClickMemory = t;
		mouseClickPos = Input.mousePosition;
		doubleClickTimeElapsed = 0f;
	}
}
