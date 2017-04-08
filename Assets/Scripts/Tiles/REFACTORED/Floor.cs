using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Normal floor tiles. Cats and dogs stand on these. Can be overridden for special floors.
/// </summary>
public class Floor : Tile {

	private GameCharacter m_occupant;
	/// <summary>
	/// The character currently standing on this tile. Automatically updated by movement of a GameCharacter.
	/// Always null for non-traversable tiles.
	/// </summary>
	public GameCharacter occupant {
		get{ return m_occupant; }
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
	override public Vector3 cursorConnectionPoint {
		get {
			if (m_occupant == null) {
				return topCenterPoint;
			}
			else {
				return new Vector3 (transform.position.x, m_occupant.elevationOfTop, transform.position.z);
			}
		}
	}


	override public bool traversable {
		get { return true; }
	}

	/// <summary>
	/// Is a cat allowed to end its movement on this square?
	/// </summary>
	public bool validPathEnd {
		get {
			return occupant == null;
		}
	}

	private PathingNode m_pathingNode;
	/// <summary>
	/// The pathing node associated with this tile. Null for walls and tiles not intended to be traversed by dogs.
	/// </summary>
	public PathingNode pathingNode {
		get{ return m_pathingNode; }
	}

	override protected void Awake () {
		base.Awake ();
		m_pathingNode = GetComponent<PathingNode> ();
	}
	
	/// <summary>
	/// Temporary and always-changing list of currently applied danger squares.
	/// </summary>
	private List<TileDangerData> visionInfo = new List<TileDangerData> ();

	/// <summary>
	/// Adds one piece of tile danger data to this tile. Changes color, enables visualizer, registers to cat.
	/// </summary>
	public void AddDangerData (TileDangerData data) {
		gridUnit.dangerVisualizerEnabled = true;
		if (m_occupant != null && m_occupant.characterType == CharacterType.Cat) {
			(m_occupant as Cat).RegisterDangerData (data);
		}
		visionInfo.Add (data);
		UpdateDangerColor ();
	}

	/// <summary>
	/// Remove the danger imposed by a specified dog.
	/// </summary>
	public void RemoveDangerDataByDog (Dog dog) {
		TileDangerData[] allData = visionInfo.ToArray ();
		foreach (TileDangerData tdd in allData) {
			if (tdd.watchingDog == dog) {
				visionInfo.Remove (tdd);
			}
		}
		if (visionInfo.Count == 0) {
			gridUnit.dangerVisualizerEnabled = false;
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
		gridUnit.dangerVisualizerEnabled = false;
		visionInfo = new List<TileDangerData> ();
	}

	/// <summary>
	/// Temporary. Only overlays the highest color.
	/// </summary>
	private void UpdateDangerColor () {
		float maxDanger = Mathf.NegativeInfinity;
		Color currentColor = Color.white;
		foreach (TileDangerData tdd in visionInfo) {
			if (tdd.danger > maxDanger) {
				maxDanger = tdd.danger;
				currentColor = tdd.dangerColor;
			}
		}
		gridUnit.dangerColor = currentColor;
	}

	/// <summary>
	/// All danger data for all dogs currently observing this tile.
	/// </summary>
	public TileDangerData[] dangerData {
		get { return visionInfo.ToArray (); }
	}

	/// <summary>
	/// This tile's danger color. Changes according to the danger data on this tile.
	/// </summary>
	public Color dangerColor {
		get{ return gridUnit.dangerColor; }
	}

	/// <summary>
	/// Is the danger visualizer active?
	/// </summary>
	public bool dangerVisualizerEnabled {
		get{ return gridUnit.dangerVisualizerEnabled; }
		set{ gridUnit.dangerVisualizerEnabled = value; }
	}

	[SerializeField] private GameObject shimmerObject;
	/// <summary>
	/// Pretty much placeholder, but works anyway. 
	/// A visual shimmer effect used for highlights other than danger squares.
	/// </summary>
	public bool shimmer {
		get { return shimmerObject.activeSelf; }
		set { 
			if (value != shimmerObject.activeSelf) {
				shimmerObject.SetActive (value);
				if (value) {
					m_shimmering.Add (this);
				}
				else {
					m_shimmering.Remove (this);
				}
			}
		}
	}

	private static HashSet<Floor> m_shimmering = new HashSet<Floor> ();
	/// <summary>
	/// A list of all shimmering tiles.
	/// </summary>
	public static Floor[] shimmeringTiles {
		get { return m_shimmering.ToArray (); }
	}

	/// <summary>
	/// Remove the shimmer effect from all tiles.
	/// </summary>
	public static void ClearAllShimmer () {
		foreach (Floor f in m_shimmering) {
			f.shimmerObject.SetActive (false);
		}
		m_shimmering = new HashSet<Floor> ();
	}

	/// <summary>
	/// Sets these tiles to be the only ones shimmering. Unshimmers all others.
	/// </summary>
	public static void MassSetShimmer (ICollection<Floor> tiles) {
		HashSet<Floor> oldTiles = m_shimmering.Clone ();
		HashSet<Floor> newTiles = new HashSet<Floor> (tiles);

		newTiles.ExceptWith (m_shimmering);
		oldTiles.ExceptWith (tiles);

		foreach (Floor f in newTiles) {
			f.shimmer = true;
		}
		foreach (Floor f in oldTiles) {
			f.shimmer = false;
		}
	}
}
