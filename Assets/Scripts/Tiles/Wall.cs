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

	public override string tileName {
		get { return "Wall. You shouldn't be seeing this."; }
	}

	public override bool allowMouseInteraction {
		get { return false; }
	}

	public override bool mouseOverVisualState {
		get { return false; }
		set { }
	}
	public override StepNode stepNode {
		get { return null; }
	}
	public override bool dangerVisualizerEnabled {
		get { return false; }
		set { }
	}
	public override bool shimmer {
		get { return false; }
		set { }
	}
	protected override void UpdateDangerColor () { }    //lel does nothing
}
