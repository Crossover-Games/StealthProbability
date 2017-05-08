using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WetFloor : ActionTile {
	public override TileType tileType {
		get { return TileType.WetFloor; }
	}

	public override string tileName {
		get { return active ? "-SPRINKLER-" : "-SPRINKLER (OFF)-"; }
	}
	public override string infoText {
		get { return active ? "Soak cats by sending them over a sprinkler! Wet cats have negated responses to risk probabilities." : "Place cats on all pressure plates in the level to activate this sprinkler."; }
	}
	public override Color infoTextColor { get { return active ? waterColor : Color.gray; } }

	public override Vector3 cursorConnectionPoint {
		get {
			if (occupant == null) {
				return topCenterPoint + Vector3.up * 0.25f;
			}
			else {
				return new Vector3 (transform.position.x, occupant.elevationOfTop, transform.position.z);
			}
		}
	}

	[SerializeField] private FloatingPowerup spinEffect;
	[SerializeField] private GameObject waterGraphic;
	/// <summary>
	/// Is this tile filled with water?
	/// </summary>
	public override bool active {
		get { return waterGraphic.activeSelf; }
	}

	public override void Activate () {
		if (!active) {
			//PlaySound ();
			spinEffect.enabled = true;
			waterGraphic.SetActive (true);
		}
	}

	/// <summary>
	/// UI color for water text.
	/// </summary>
	public static Color waterColor {
		get { return new Color (0f, 0.58f, 1f); }
	}
}
