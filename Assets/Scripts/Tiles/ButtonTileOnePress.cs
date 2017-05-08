using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTileOnePress : Floor {

	private static ButtonTileOnePress [] allButtonTiles;
	[SerializeField] private GameObject activatedObject;
	[SerializeField] private Material activatedMaterial;
	[SerializeField] private GameObject buttonParent;
	private Renderer [] rends;
	[SerializeField] private FloatingPowerup spinEffect;

	[SerializeField] private ActionTile [] dependentTiles;
	private bool m_actionExecuted;

	/// <summary>
	/// Have the sprinklers been turned on yet?
	/// </summary>
	public bool actionAlreadyExecuted {
		get { return m_actionExecuted; }
	}

	public override string tileName {
		get { return "-BUTTON-"; }
	}

	public override string infoText {
		get { return m_actionExecuted ? "This button was successfully activated." : "If a cat steps on this button, something special will happen!"; }
	}
	public override Color infoTextColor {
		get { return m_actionExecuted ? Color.green.OptimizedForText () : Color.white; }
	}

	protected override void LateAwake () {
		base.LateAwake ();
		m_actionExecuted = false;
		rends = buttonParent.GetComponentsInChildren<Renderer> ();
	}

	public void ActivateVisuals () {
		activatedObject.SetActive (true);
		m_actionExecuted = true;
		spinEffect.enabled = true;
		foreach (Renderer r in rends) {
			r.material = activatedMaterial;
		}
	}

	/// <summary>
	/// Turn on all sprinklers.
	/// </summary>
	public void ActivateAll () {
		foreach (ActionTile w in dependentTiles) {
			w.Activate ();
		}
	}

	/// <summary>
	/// Focus the camera on the sprinklers.
	/// </summary>
	public void CameraToFocusPoint () {
		CameraOverheadControl.SetCamFocusPoint (dependentTiles [0].topCenterPoint);
	}
}
