using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WetFloor : Floor {
	public override TileType tileType {
		get { return TileType.WetFloor; }
	}

	public override string tileName {
		get { return flooded ? "-WET FLOOR-" : "-FLOOR-"; }
	}

	[SerializeField] private GameObject waterGraphic;
	/// <summary>
	/// Is this tile filled with water?
	/// </summary>
	public bool flooded {
		get { return waterGraphic.activeSelf; }
		set { waterGraphic.SetActive (value); }
	}

	/// <summary>
	/// UI color for water text.
	/// </summary>
	public static Color waterColor {
		get { return new Color (0f, 0.58f, 1f); }
	}
}
