using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Refactoring dat tile doe 
/// </summary>
public abstract class Tile : MonoBehaviour {
	[Tooltip ("Set a reference to its grid square visualizer object.")]
	/// <summary>
	/// Controls mouse over, click, grid square
	/// </summary>
	[SerializeField] protected TileGridUnitVisualizer gridUnit;
	/// <summary>
	/// State of the tile's highlight. Encapsulates field for TileGridUnitVisualizer.
	/// </summary>
	public bool mouseOverVisualState {
		get{ return gridUnit.mouseOverVisualState; }
		set{ gridUnit.mouseOverVisualState = value; }
	}

	[SerializeField] private TileType m_tileType;
	public TileType tileType {
		get{ return m_tileType; }
	}

	[SerializeField] private Collider m_collider;
	/// <summary>
	/// Returns the world space coordinate of center top of tile. This is where the occupying character will stand, where the cursor will point on an empty tile, etc.
	/// </summary>
	public Vector3 topCenterPoint {
		get { return new Vector3 (transform.position.x, m_collider.bounds.max.y, transform.position.z); }
	}
	/// <summary>
	/// Returns the highest point on the tile where the arrow cursor will be. Will be either over a character's head or over simple ground.
	/// </summary>
	abstract public Vector3 cursorConnectionPoint { get; }

	/// <summary>
	/// Can characters stand on this?
	/// </summary>
	abstract public bool traversable { get; }

	// neighboring tiles
	private Tile northTile = null;
	private Tile southTile = null;
	private Tile westTile = null;
	private Tile eastTile = null;

	virtual protected void Awake () {
		RegisterNeighboringTiles ();

		gridUnit.AssociateTile (this);
		TileManager.RegisterTileSetup (this);
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
	/// Relative to this tile, in what direction would you move to step on the other tile? Returns north as a failsafe, but please don't even call this if you know the other isn't a neighbor.
	/// </summary>
	public Compass.Direction GetDirectionOfNeighbor (Tile other) {
		foreach (Compass.Direction direction in Compass.allDirections) {
			if (GetNeighborInDirection (direction) == other) {
				return direction;
			}
		}
		return Compass.Direction.North;
	}

	/// <summary>
	/// Returns a list of all neighboring tiles.
	/// </summary>
	public List<Tile> allNeighbors {
		get {
			List<Tile> tmp = new List<Tile> ();
			foreach (Compass.Direction direction in Compass.allDirections) {
				Tile possibleNeighbor = GetNeighborInDirection (direction);
				if (possibleNeighbor != null) {
					tmp.Add (possibleNeighbor);
				}
			}
			return tmp;
		}
	}

	/// <summary>
	/// All tiles in radius. Radius 0 is just this tile, radius 1 is neighbors only, etc.
	/// </summary>
	public List<Tile> AllTilesInRadius (int radius, bool traversableOnly, bool includeSelf) {
		HashSet<Tile> all = new HashSet<Tile> ();
		all.Add (this);
		for (int x = 0; x < radius; x++) {
			HashSet<Tile> tempAll = all.Clone ();
			foreach (Tile t in all) {
				foreach (Tile n in t.allNeighbors) {
					if (n.traversable || !traversableOnly) {
						tempAll.Add (n);
					}
				}
			}
			all = tempAll;
		}
		if (!includeSelf) {
			all.Remove (this);
		}
		return all.ToList ();
	}

	/// <summary>
	/// All tiles in radius. Radius 0 is just this tile, radius 1 is neighbors only, etc.
	/// </summary>
	public List<Floor> AllTraversableFloorsInRadius (int radius, bool traversableOnly, bool includeSelf) {
		HashSet<Tile> all = new HashSet<Tile> ();
		all.Add (this);
		for (int x = 0; x < radius; x++) {
			HashSet<Tile> tempAll = all.Clone ();
			foreach (Tile t in all) {
				foreach (Tile n in t.allNeighbors) {
					if (n.traversable || !traversableOnly) {
						tempAll.Add (n);
					}
				}
			}
			all = tempAll;
		}
		if (!includeSelf) {
			all.Remove (this);
		}
		return all.ToList ();
	}

	/// <summary>
	/// Is this tile traversable and not null?
	/// </summary>
	public static bool IsValidMoveDestination (Tile t) {
		return t != null && t.traversable;
	}
}
