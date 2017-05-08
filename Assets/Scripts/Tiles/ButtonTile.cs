using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTile : Floor {
	private static List<ButtonTile> allButtons;

	private static ActionTile [] allActionTiles;
	private static bool m_actionExecuted;

	/// <summary>
	/// Have the sprinklers been turned on yet?
	/// </summary>
	public static bool actionAlreadyExecuted {
		get { return m_actionExecuted; }
	}

	public override string tileName {
		get { return "-PRESSURE PLATE-"; }
	}

	public override string infoText {
		get { return "If all pressure plates are simultaneously occupied by different cats, something special will happen!"; }
	}

	protected override void LateAwake () {
		base.LateAwake ();
		allButtons = new List<ButtonTile> ();
		allActionTiles = null;
		m_actionExecuted = false;
	}

	protected override void LateStart () {
		allButtons.Add (this);
		if (allActionTiles == null) {
			allActionTiles = GameObject.FindObjectsOfType<ActionTile> ();
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
	public static void ActivateAll () {
		m_actionExecuted = true;
		foreach (ActionTile w in allActionTiles) {
			w.Activate ();
		}
	}

	/// <summary>
	/// Focus the camera on the sprinklers.
	/// </summary>
	public static void CameraToFocusPoint () {
		CameraOverheadControl.SetCamFocusPoint (allActionTiles [0].topCenterPoint);
	}
}
