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
		if (secondRiskiest.danger < 0f) {
			m_dangerVisualizer.tileDangerColor = riskiest.dangerColor;
			m_dangerVisualizer.hologramColor = riskiest.dangerColor;
		}
		else {
			m_dangerVisualizer.tileDangerColor = riskiest.dangerColor;
			m_dangerVisualizer.hologramColor = secondRiskiest.dangerColor;
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
	protected override bool cosmeticShimmerState {
		get { return shimmerObject.activeSelf; }
		set { shimmerObject.SetActive (value); }
	}
}

