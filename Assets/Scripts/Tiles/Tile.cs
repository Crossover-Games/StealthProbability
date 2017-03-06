﻿using UnityEngine;
using System.Collections;

/// <summary>
/// The game board is made up of these! IMPORTANT: Heavily dependent on the convention that a tile is 1x1x1 units and spaced accordingly. Remember to hold CTRL while dragging an object in the editor.
/// </summary>
public class Tile : MonoBehaviour {

	// universal tile manager
	private UniversalTileManager theManager;
	/// <summary>
	/// Reference to the scene's universal tile manager.
	/// </summary>
	public UniversalTileManager tileManager {
		get{ return theManager; }
	}

	// visualization
	[Tooltip ("Set a reference to its grid square visualizer object.")]
	[SerializeField] private TileGridUnitVisualizer visualizer;

	// neighboring tiles
	private Tile northTile = null;
	private Tile southTile = null;
	private Tile westTile = null;
	private Tile eastTile = null;

	private Collider myCollider;

	[SerializeField] private TileType myTileType;
	public TileType tileType {
		get{ return myTileType; }
	}
		
	[SerializeField] private GameCharacter myOccupant;
	/// <summary>
	/// The character currently standing on this tile. Automatically updated by movement of a GameCharacter.
	/// </summary>
	public GameCharacter occupant {
		get{ return myOccupant; }
	}
		
	/// <summary>
	/// Should only be called by GameCharacter and its subtypes. Please don't tinker with this.
	/// </summary>
	public void SetOccupant (GameCharacter gc) {
		myOccupant = gc;
	}

	/// <summary>
	/// Returns the highest point on the tile where the arrow cursor will be. Will be either over a character's head or over simple ground.
	/// </summary>
	public Vector3 cursorConnectionPoint {
		get {
			if (myOccupant == null) {
				return characterConnectionPoint;
			}
			else {
				return new Vector3 (transform.position.x, myOccupant.elevationOfTop, transform.position.z);
			}
		}
	}

	private PathingNode myPathingNode;
	/// <summary>
	/// The pathing node associated with this tile. Null for walls and tiles not intended to be traversed by dogs.
	/// </summary>
	public PathingNode pathingNode {
		get{ return myPathingNode; }
	}

	/// <summary>
	/// Returns the center of the top of tile where the occupying character will stand.
	/// </summary>
	public Vector3 characterConnectionPoint {
		get { return new Vector3 (transform.position.x, myCollider.bounds.max.y, transform.position.z); }
	}

	void Awake () {
		theManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<UniversalTileManager> ();
		myCollider = GetComponent<Collider> ();
		myPathingNode = GetComponent<PathingNode> ();

		RegisterNeighboringTiles ();

		visualizer.AssociateTile (this);
		theManager.RegisterTileSetup (this);
	}

	void Start () {
		//
	}

	private void RegisterNeighboringTiles () {
		RaycastHit hit;
		Vector3 rayOrigin = transform.position + Vector3.down * 100;

		if (Physics.Raycast (rayOrigin + Vector3.forward, Vector3.up, out hit)) {
			northTile = TileRaycastHelper (hit);
		}
		if (Physics.Raycast (rayOrigin + Vector3.back, Vector3.up, out hit)) {
			southTile = TileRaycastHelper (hit);
		}
		if (Physics.Raycast (rayOrigin + Vector3.right, Vector3.up, out hit)) {
			eastTile = TileRaycastHelper (hit);
		}
		if (Physics.Raycast (rayOrigin + Vector3.left, Vector3.up, out hit)) {
			westTile = TileRaycastHelper (hit);
		}
	}
	private Tile TileRaycastHelper (RaycastHit hit) {
		Tile tileTemp = hit.collider.gameObject.GetComponent<Tile> ();
		if (tileTemp != null) {
			return tileTemp;
		}
		else {
			return null;
		}
	}

	/// <summary>
	/// Returns the neighboring tile in the specified direction.
	/// </summary>
	public Tile GetNeighborInDirection (Compass.Direction direction) {
		switch (direction) {
			case Compass.Direction.North:
				return northTile;
			case Compass.Direction.South:
				return southTile;
			case Compass.Direction.East:
				return eastTile;
			case Compass.Direction.West:
				return westTile;
			default:
				return null;
		}
	}

	/// <summary>
	/// Checks if another tile is a neighbor to this one. Diagonals are not neighbors.
	/// </summary>
	public bool IsNeighbor (Tile other) {
		foreach (Compass.Direction direction in Compass.allDirections) {
			if (GetNeighborInDirection (direction) == other) {
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// State of the tile's highlight. Encapsulates field for TileGridUnitVisualizer.
	/// </summary>
	public bool mouseOverVisualState {
		get{ return visualizer.mouseOverVisualState; }
		set{ visualizer.mouseOverVisualState = value; }
	}

	/// <summary>
	/// This tile's danger color.
	/// </summary>
	public Color dangerColor {
		get{ return visualizer.dangerColor; }
		set{ visualizer.dangerColor = value; }
	}

	/// <summary>
	/// Is the danger visualizer active?
	/// </summary>
	public bool dangerVisualizerEnabled {
		get{ return visualizer.dangerVisualizerEnabled; }
		set{ visualizer.dangerVisualizerEnabled = value; }
	}
}
