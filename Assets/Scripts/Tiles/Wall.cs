using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Normal static wall tiles. Characters cannot stand on these.
/// </summary>
public class Wall : Tile {

	public override TileType tileType {
		get { return TileType.Wall; }
	}

	public override GameCharacter occupant {
		get { return null; }
	}

	public override bool traversable {
		get { return false; }
	}

	public override bool allowMouseInteraction {
		get { return false; }
	}

	public override bool mouseOverVisualState {
		get { return false; }
		set { }
	}
	public override Color dangerColor {
		get { return Color.clear; }
		set { }
	}
	public override bool dangerVisualizerEnabled {
		get { return false; }
		set { }
	}
	protected override bool cosmeticShimmerState {
		get { return false; }
		set { }
	}
}
