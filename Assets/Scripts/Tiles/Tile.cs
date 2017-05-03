using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The game board is made up of these! IMPORTANT: Heavily dependent on the convention that a tile is 1x1x1 units and spaced accordingly. Remember to hold CTRL while dragging an object in the editor.
/// </summary>
public abstract class Tile : MonoBehaviour {

	// neighboring tiles
	private Tile northTile = null;
	private Tile southTile = null;
	private Tile westTile = null;
	private Tile eastTile = null;

	private Collider m_collider;

	/// <summary>
	/// What type of tile is this?
	/// </summary>
	public abstract TileType tileType { get; }

	/// <summary>
	/// Can this tile be stepped on?
	/// </summary>
	public abstract bool traversable { get; }

	/// <summary>
	/// The name that will appear when this tile is selected.
	/// </summary>
	public abstract string tileName { get; }

	/// <summary>
	/// Can a cat end its movement on this square?
	/// </summary>
	public bool validMoveEnd {
		get { return traversable && occupant == null; }
	}

	private GameCharacter m_occupant;
	/// <summary>
	/// The character currently standing on this tile. Automatically updated by movement of a GameCharacter.
	/// </summary>
	public virtual GameCharacter occupant {
		get { return m_occupant; }
	}

	/// <summary>
	/// Should only be called by GameCharacter and its subtypes. Please don't tinker with this.
	/// </summary>
	public void SetOccupant (GameCharacter gc) {
		m_occupant = gc;
	}

	/// <summary>
	/// Returns the highest point on the tile where the arrow cursor will be. Will be either over a character's head or over simple ground.
	/// </summary>
	public virtual Vector3 cursorConnectionPoint {
		get {
			if (occupant == null) {
				return topCenterPoint;
			}
			else {
				return new Vector3 (transform.position.x, m_occupant.elevationOfTop, transform.position.z);
			}
		}
	}

	/// <summary>
	/// Returns the world space coordinate of center top of tile. This is where the occupying character will stand, where the cursor will point on an empty tile, etc.
	/// </summary>
	public virtual Vector3 topCenterPoint {
		get { return new Vector3 (transform.position.x, m_collider.bounds.max.y, transform.position.z); }
	}

	void Awake () {
		m_collider = GetComponent<Collider> ();

		RegisterNeighboringTiles ();
		TileManager.RegisterTileSetup (this);
		LateAwake ();
	}

	/// <summary>
	/// Called at the end of awake, but still before start.
	/// </summary>
	protected virtual void LateAwake () {
		// nothing by default
	}

	/// <summary>
	/// Called at the end of start.
	/// </summary>
	protected virtual void LateStart () {
		// nothing by default
	}

	void Start () {
		if (!allowMouseInteraction) {
			gameObject.MoveToIgnoreRaycastLayer ();
		}
		LateStart ();
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
	/// Can this tile be clicked and moused over?
	/// </summary>
	public abstract bool allowMouseInteraction { get; }

	/// <summary>
	/// State of the tile's mouse highlight.
	/// </summary>
	public abstract bool mouseOverVisualState { get; set; }

	/// <summary>
	/// The step node associated with this tile.
	/// </summary>
	public abstract StepNode stepNode { get; }

	/// <summary>
	/// Temporary and always-changing list of currently applied danger squares.
	/// </summary>
	private List<TileDangerData> visionInfo = new List<TileDangerData> ();

	/// <summary>
	/// Adds one piece of tile danger data to this tile. Changes color, enables visualizer, registers to cat.
	/// </summary>
	public void AddDangerData (TileDangerData data) {
		dangerVisualizerEnabled = true;
		visionInfo.Add (data);
		UpdateDangerColor ();
	}

	/// <summary>
	/// Remove the danger imposed by a specified dog.
	/// </summary>
	public void RemoveDangerDataByDog (Dog dog) {
		TileDangerData [] allData = visionInfo.ToArray ();
		foreach (TileDangerData tdd in allData) {
			if (tdd.watchingDog == dog) {
				visionInfo.Remove (tdd);
			}
		}
		if (visionInfo.Count == 0) {
			dangerVisualizerEnabled = false;
		}
		else {
			UpdateDangerColor ();
		}
	}

	/// <summary>
	/// Practically, there should be no need to call this.
	/// Remove all tile danger data elements from this tile. Also removes the visualizer.
	/// </summary>
	public void ClearAllDangerData () {
		dangerVisualizerEnabled = false;
		visionInfo = new List<TileDangerData> ();
	}

	/// <summary>
	/// Called when danger is applied to determine the appearance of the danger tile.
	/// </summary>
	protected abstract void UpdateDangerColor ();

	/// <summary>
	/// All danger data for all dogs currently observing this tile.
	/// </summary>
	public TileDangerData [] dangerData {
		get { return visionInfo.ToArray (); }
	}

	/// <summary>
	/// Is the danger visualizer active?
	/// </summary>
	public abstract bool dangerVisualizerEnabled { get; set; }

	/// <summary>
	/// A visual shimmer effect used for highlights other than danger squares.
	/// </summary>
	public abstract bool shimmer { get; set; }

	/// <summary>
	/// All tiles in radius. Radius 0 is just this tile. TraversableOnly means that walls or filled squares won't be included.
	/// </summary>
	public List<Tile> AllTilesInRadius (int radius, bool traversableOnly, bool includeSelf) {
		HashSet<Tile> all = new HashSet<Tile> ();
		all.Add (this);
		for (int x = 0; x < radius; x++) {
			HashSet<Tile> tempAll = all.Clone ();
			foreach (Tile t in all) {
				foreach (Tile n in t.allNeighbors) {
					if (t.traversable || !traversableOnly) {
						tempAll.Add (n);
					}
				}
				all = tempAll;
			}
			if (!includeSelf) {
				all.Remove (this);
			}
		}
		return all.ToList ();
	}

	/// <summary>
	/// Not null and traversable
	/// </summary>
	public static bool ValidStepDestination (Tile t) {
		return t != null && t.traversable;
	}

}