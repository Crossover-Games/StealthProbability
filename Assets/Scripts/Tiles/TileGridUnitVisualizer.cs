using UnityEngine;
using System.Collections;

//TODO: gui overlay of the information
// eventually, this should alert the tile it's connected to, or at least get data from it


/// <summary>
/// Representation of the board space a single tile takes up. Will likely also handle the GUI overlay detailing statistics like illumination of the tile, etc...
/// </summary>
[RequireComponent (typeof(Renderer))]
public class TileGridUnitVisualizer : MonoBehaviour {
	private Renderer myRenderer;
	private Tile myTile;
	public Tile associatedTile {
		get{ return myTile; }
	}

	/// <summary>
	/// If enabled, this unit will react to being moused over. Not intended to be changed at any time but Awake.
	/// </summary>
	public bool mouseOverEnabled = true;

	[SerializeField] private Material mouseOverMaterial;
	[SerializeField] private Material mouseAwayMaterial;
	[SerializeField] private DangerSquareVisualizer dangerVisualizer;

	void Awake () {
		myRenderer = GetComponent<Renderer> ();
	}

	/// <summary>
	/// Only to be used in setup. Connects the grid visualization of the tile to the actual tile object.
	/// </summary>
	public void AssociateTile (Tile t) {
		myTile = t;
		dangerColor = DangerSquareVisualizer.WHITE;
	}

	void OnMouseEnter () {
		if (mouseOverEnabled) {
			TileManager.RegisterMouseEnter (myTile);
		}
	}
	void OnMouseExit () {
		TileManager.CheckIfUnregisterIsRequired (myTile);
	}

	private bool mouseOverVisual = false;
	/// <summary>
	/// Is this tile's mouse over visual active?
	/// </summary>
	public bool mouseOverVisualState {
		get{ return mouseOverVisual; }
		set {
			if (mouseOverVisual != value) {
				mouseOverVisual = value;
				if (mouseOverVisual) {
					myRenderer.material = mouseOverMaterial;
				}
				else {
					myRenderer.material = mouseAwayMaterial;
				}
			}
		}
	}

	/// <summary>
	/// This square's danger color. Can still be updated if the dangerVisualizerState is off, but will not be displayed until dangerVisualizerState is turned on again.
	/// </summary>
	public Color dangerColor {
		get{ return dangerVisualizer.color; }
		set{ dangerVisualizer.color = value; }
	}

	/// <summary>
	/// Show this tile's danger color? dangerColor can still be changed when this is off, but will not be seen until this is turned on again
	/// </summary>
	public bool dangerVisualizerEnabled {
		get{ return dangerVisualizer.gameObject.activeSelf; }
		set {
			if (dangerVisualizer.gameObject.activeSelf != value) {
				dangerVisualizer.gameObject.SetActive (value);
			}
		}
	}
}
