using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTile : Floor {
	private static List<ButtonTile> allButtons;

	private static WetFloor [] allWetFloors;
	private static bool m_actionExecuted;

	/// <summary>
	/// Have the sprinklers been turned on yet?
	/// </summary>
	public static bool actionAlreadyExecuted {
		get { return m_actionExecuted; }
	}

	protected override void LateAwake () {
		base.LateAwake ();
		allButtons = new List<ButtonTile> ();
		allWetFloors = null;
		m_actionExecuted = false;
	}

	protected override void LateStart () {
		allButtons.Add (this);
		if (allWetFloors == null) {
			allWetFloors = GameObject.FindObjectsOfType<WetFloor> ();
		}
	}

	/// <summary>
	/// Are all buttons occupied?
	/// </summary>
	public static bool AllButtonsActivated () {
		foreach (ButtonTile b in allButtons) {
			if (b.occupant == null) {
				return false;
			}
		}
		return true;
	}

	/// <summary>
	/// Turn on all sprinklers.
	/// </summary>
	public static void Activate () {
		m_actionExecuted = true;
		foreach (WetFloor w in allWetFloors) {
			w.flooded = true;
		}
	}

	/// <summary>
	/// Focus the camera on the sprinklers.
	/// </summary>
	public static void CameraToSprinklers () {
		CameraOverheadControl.SetCamFocusPoint (allWetFloors [0].topCenterPoint);
	}
}
