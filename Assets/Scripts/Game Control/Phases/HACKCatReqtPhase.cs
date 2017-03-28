using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A piece of crap that is only going to be used in our client demo on Tuesday, March 28.
/// </summary>
public class HACKCatReqtPhase : GameControlPhase {

	public Unfreeze2DRigidbodies unfreezer;
	private bool onceOnly;

	/// <summary>
	/// The rekt cat.
	/// </summary>
	public Cat rektCat;

	public override void OnTakeControl () {
		print ("HACKED MY WAY INTO THIS ONE");
		onceOnly = true;
		brain.cameraControl.SetCamFocusPoint (rektCat.myTile.topCenterPoint);
	}

	public override void ControlUpdate () {
		rektCat.transform.localScale = Vector3.Lerp (rektCat.transform.localScale, Vector3.zero, 2 * Time.deltaTime);
		if (onceOnly && rektCat.transform.localScale.magnitude < 0.25f) {
			onceOnly = false;
			unfreezer.Unfreeze ();
		}
	}
}
