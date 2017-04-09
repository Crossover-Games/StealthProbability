using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Regular floors. Characters can stand on these.
/// </summary>
public class Floor : Tile {

	public override TileType tileType {
		get { return TileType.Floor; }
	}

	public override bool traversable {
		get { return true; }
	}

	public override bool allowMouseInteraction {
		get { return true; }
	}

	[SerializeField] private TileMouseUnit m_mouseUnit;
	public override bool mouseOverVisualState {
		get { return m_mouseUnit.mouseOverVisualState; }
		set { m_mouseUnit.mouseOverVisualState = value; }
	}

	[SerializeField] private TileDangerVisualizer m_dangerVisualizer;
	public override Color dangerColor {
		get { return m_dangerVisualizer.color; }
		set { m_dangerVisualizer.color = value; }
	}
	public override bool dangerVisualizerEnabled {
		get { return m_dangerVisualizer.gameObject.activeSelf; }
		set {
			if (m_dangerVisualizer.gameObject.activeSelf != value) {
				m_dangerVisualizer.gameObject.SetActive (value);
			}
		}
	}

	[SerializeField] private GameObject shimmerObject;
	protected override bool cosmeticShimmerState {
		get { return shimmerObject.activeSelf; }
		set { shimmerObject.SetActive (value); }
	}
}

