using UnityEngine;
using System.Collections;

//TODO: gui overlay of the information
// eventually, this should alert the tile it's connected to, or at least get data from it


/// <summary>
/// Representation of the board space a single tile takes up. Will also handle the GUI overlay detailing statistics like illumination of the tile, etc...
/// </summary>
[RequireComponent (typeof(Renderer))]
public class TileGridUnitVisualizer : MonoBehaviour {
	private Renderer myRenderer;
	private Tile myTile;
	public Tile associatedTile {
		get{ return myTile; }
	}

	[Tooltip ("Set a reference to .")]
	[SerializeField] private Collider myCollider;
	public float elevationOfTop {
		get {
			return myCollider.bounds.max.y;
		}
	}

	[SerializeField] private Material unhighlighted;
	[SerializeField] private Material highlighted;
	[SerializeField] private GameObject selectionHighlight;

	void Awake () {
		myRenderer = GetComponent<Renderer> ();
	}
		
	void OnMouseEnter () {
		myTile.tileManager.RegisterMouseOver (myTile);
		myRenderer.material = highlighted;
	}
	void OnMouseExit () {
		myTile.tileManager.CheckIfUnregisterIsRequired (myTile);
		myRenderer.material = unhighlighted;
	}

	/// <summary>
	/// Only to be used in setup. Connects the grid visualization of the tile to the actual tile object.
	/// </summary>
	public void AssociateTile (Tile t) {
		myTile = t;
	}


	/// <summary>
	/// Gets or sets the tile's highlighted ground state.
	/// </summary>
	public bool highlightState {
		get{ return selectionHighlight.activeSelf; }
		set{ selectionHighlight.SetActive (value); }
	}

}
