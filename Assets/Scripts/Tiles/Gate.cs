using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : ActionTile {

	public override TileType tileType {
		get { return active ? TileType.Wall : TileType.Floor; }
	}

	public override bool traversable {
		get { return active; }
	}

	public override string tileName {
		get { return active ? "-GATE (LIFTED)-" : "-GATE-"; }
	}
	public override string infoText {
		get { return active ? "" : "This gate requires cat input to be lifted."; }
	}

	public override Vector3 cursorConnectionPoint {
		get {
			if (occupant == null) {
				if (active) {
					return topCenterPoint;
				}
				else {
					return topCenterPoint + Vector3.up;
				}
			}
			else {
				return new Vector3 (transform.position.x, occupant.elevationOfTop, transform.position.z);
			}
		}
	}

	protected override void LateAwake () {
		base.LateAwake ();
		m_active = false;
	}

	private bool m_active;
	[SerializeField] private GameObject physicalGate;
	/// <summary>
	/// Is this tile filled with water?
	/// </summary>
	public override bool active {
		get { return m_active; }
	}

	public override void Activate () {
		if (!active) {
			AnimationManager.AddAnimation (physicalGate.transform, new AnimationDestination (physicalGate.transform.position + Vector3.up, null, null, 0.5f, InterpolationMethod.Sinusoidal));
			m_active = true;
		}
	}
}
