using UnityEngine;
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


	[SerializeField] private TileType tileType;
	public TileType MyTileType {
		get{ return tileType; }
	}

	// TODO: data
	// this will include info like illumination and other things related to probability


	[SerializeField] private GameCharacter occupant = null;
	/// <summary>
	/// The character currently standing on this tile.
	/// </summary>
	public GameCharacter occupyingCharacter {
		get{ return occupant; }
		//set{ occupant = value; }
	}
		
	/// <summary>
	/// Returns the highest point on the tile where the arrow cursor will be. Will be either over a character's head or over simple ground. Not yet programmed to do the former though. Encapsulates field for TileGridUnitVisualizer.
	/// </summary>
	public Vector3 cursorConnectionPoint {
		get {
			if (occupant == null) {
				return new Vector3 (transform.position.x, visualizer.elevationOfTop, transform.position.z);
			}
			else {
				return new Vector3 (transform.position.x, occupant.elevationOfTop, transform.position.z);
			}
		}
	}

	void Awake () {
		theManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<UniversalTileManager> ();
		visualizer.AssociateTile (this);
	}

	void Start () {
		RegisterNeighboringTiles ();
	}

	private void RegisterNeighboringTiles () {
		RaycastHit hit;
		Vector3 rayOrigin = transform.position + Vector3.up * 100;

		if (Physics.Raycast (rayOrigin + Vector3.forward, Vector3.down, out hit)) {
			northTile = TileRaycastHelper (hit);
		}
		if (Physics.Raycast (rayOrigin + Vector3.back, Vector3.down, out hit)) {
			southTile = TileRaycastHelper (hit);
		}
		if (Physics.Raycast (rayOrigin + Vector3.right, Vector3.down, out hit)) {
			eastTile = TileRaycastHelper (hit);
		}
		if (Physics.Raycast (rayOrigin + Vector3.left, Vector3.down, out hit)) {
			westTile = TileRaycastHelper (hit);
		}
	}
	private Tile TileRaycastHelper (RaycastHit hit) {
		TileGridUnitVisualizer tguv = hit.collider.gameObject.GetComponent<TileGridUnitVisualizer> ();
		if (tguv != null) {
			return hit.collider.gameObject.GetComponent<TileGridUnitVisualizer> ().associatedTile;
		}
		else {
			return null;
		}
	}

	/// <summary>
	/// Returns the neighboring tile in the specified direction.
	/// </summary>
	public Tile GetTileInDirection (CardinalDirection direction) {
		switch (direction) {
			case CardinalDirection.North:
				return northTile;
			case CardinalDirection.South:
				return southTile;
			case CardinalDirection.East:
				return eastTile;
			case CardinalDirection.West:
				return westTile;
			default:
				return null;
		}
	}

	/// <summary>
	/// State of the tile's highlight. Encapsulates field for TileGridUnitVisualizer.
	/// </summary>
	public bool highlightState {
		get{ return visualizer.highlightState; }
		set{ visualizer.highlightState = value; }
	}
}
