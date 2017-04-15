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

	public override string tileName {
		get { return "-FLOOR-"; }
	}

	public override bool allowMouseInteraction {
		get { return true; }
	}

	[SerializeField] private TileDangerVisualizer m_dangerVisualizer;
	protected override void UpdateDangerColor () {
		TileDangerData riskiest = new TileDangerData (Mathf.NegativeInfinity, null, null, default (Color));
		TileDangerData secondRiskiest = riskiest;
		foreach (TileDangerData tdd in dangerData) {
			if (tdd.danger > riskiest.danger) {
				secondRiskiest = riskiest;
				riskiest = tdd;
			}
		}
		m_dangerVisualizer.hologramColor = riskiest.dangerColor;
		if (dangerData.Length > 1) {
			m_dangerVisualizer.tileDangerColor = Color.black;
		}
		else {
			m_dangerVisualizer.tileDangerColor = riskiest.dangerColor;
		}
	}

	[SerializeField] private TileMouseUnit m_mouseUnit;
	public override bool mouseOverVisualState {
		get { return m_mouseUnit.mouseOverVisualState; }
		set { m_mouseUnit.mouseOverVisualState = value; }
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
	public override bool shimmer {
		get { return shimmerObject.activeSelf; }
		set {
			if (value != shimmerObject.activeSelf) {
				shimmerObject.SetActive (value);
				if (value) {
					TileManager.RegisterShimmer (this);
				}
				else {
					TileManager.UnregisterShimmer (this);
				}
			}
		}
	}
}

