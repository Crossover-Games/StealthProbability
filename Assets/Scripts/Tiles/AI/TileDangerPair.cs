using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains a tile and its associated danger value, as projected by the VisionPattern this belongs to.
/// This structure knows nothing about other modifiers on the tile, such as other dogs looking at it.
/// </summary>
public struct TileDangerPair {

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

	public TileDangerPair (float dangerValue, Tile tile) {
		m_Danger = dangerValue;
		m_Tile = tile;
	}
}
