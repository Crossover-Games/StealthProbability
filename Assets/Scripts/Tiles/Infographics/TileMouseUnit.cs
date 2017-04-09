using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the mouse over and click for the tile. Additionally, draws the grid square.
/// </summary>
public class TileMouseUnit : MonoBehaviour {
	private Renderer myRenderer;
	[SerializeField] private Tile myTile;

	[SerializeField] private Material mouseOverMaterial;
	[SerializeField] private Material mouseAwayMaterial;
	
	
	void Awake () {
		myRenderer = GetComponent<Renderer> ();
	}

	void OnMouseEnter () {
		TileManager.RegisterMouseEnter (myTile);
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
}

