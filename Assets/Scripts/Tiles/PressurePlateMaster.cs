using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateMaster : MonoBehaviour {

	private static PressurePlateMaster staticInstance;

	private static PressurePlate [] allPlates;

	private static ActionTile [] dependentTiles;

	private static bool m_actionExecuted;
	/// <summary>
	/// Have the sprinklers been turned on yet?
	/// </summary>
	public static bool actionAlreadyExecuted {
		get { return m_actionExecuted; }
	}

	void Awake () {
		m_actionExecuted = false;
		allPlates = FindObjectsOfType<PressurePlate> ();
		dependentTiles = FindObjectsOfType<ActionTile> ();
		staticInstance = this;
	}

	/// <summary>
	/// Are all buttons occupied?
	/// </summary>
	public static bool AllButtonsActivated () {
		if (allPlates.Length == 0) {
			return false;
		}
		foreach (PressurePlate b in allPlates) {
			if (b.occupant == null) {
				return false;
			}
		}
		return true;
	}


	/// <summary>
	/// Turn on all sprinklers.
	/// </summary>
	public static void ActivateAll () {
		foreach (PressurePlate p in allPlates) {
			p.ApplyCompleteVisuals ();
		}
		m_actionExecuted = true;
		foreach (ActionTile w in dependentTiles) {
			w.Activate ();
		}
	}

	/// <summary>
	/// Focus the camera on the first one.
	/// </summary>
	public static void CameraToFocusPoint () {
		CameraOverheadControl.SetCamFocusPoint (dependentTiles [0].topCenterPoint);
	}
}
