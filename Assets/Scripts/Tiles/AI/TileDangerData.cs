using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Relates a probability to a tile, with respect to a single vision pattern. Also includes the danger color of that square.
/// This structure knows nothing about other modifiers on the tile, such as other dogs looking at it.
/// </summary>
public class TileDangerData {

	private float m_Danger;
	/// <summary>
	/// The base likelihood of a cat being detected by a specific dog on this square. Probably should be between 0 and 1.
	/// </summary>
	public float danger {
		get { return m_Danger; }
	}

	private Tile m_Tile;
	/// <summary>
	/// The tile this pair is associated with.
	/// </summary>
	public Tile myTile {
		get { return m_Tile; }
	}

	private Dog m_Dog;
	/// <summary>
	/// The dog this vision pattern belongs to.
	/// </summary>
	public Dog watchingDog {
		get { return m_Dog; }
	}

	private Color m_Color;
	/// <summary>
	/// The color used to visualize the danger of this tile. 
	/// </summary>
	public Color dangerColor {
		get { return m_Color; }
	}

	/// <summary>
	/// Constructs a new TileDangerData. These values cannot be changed.
	/// </summary>
	public TileDangerData (float dangerValue, Tile tile, Dog dog, Color color) {
		m_Danger = dangerValue;
		m_Tile = tile;
		m_Dog = dog;
		m_Color = color;
	}
}
