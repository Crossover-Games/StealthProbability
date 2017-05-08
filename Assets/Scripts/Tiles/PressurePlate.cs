using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Floor {
	[SerializeField] private GameObject activatedObject;
	[SerializeField] private Material activatedMaterial;

	public override string tileName {
		get { return "-PRESSURE PLATE-"; }
	}

	public override string infoText {
		get { return "If all pressure plates are simultaneously occupied by different cats, something special will happen!"; }
	}

	private Renderer [] rends;
	[SerializeField] private GameObject buttonParent;

	protected override void LateAwake () {
		base.LateAwake ();

		rends = buttonParent.GetComponentsInChildren<Renderer> ();
	}

	public void ApplyCompleteVisuals () {
		foreach (Renderer r in rends) {
			r.material = activatedMaterial;
		}
		activatedObject.SetActive (true);
	}
}
