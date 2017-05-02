using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCameraCommand : IActionCommand {
	private Vector3 point;
	public PointCameraCommand (Vector3 point) {
		this.point = point;
	}
	public void Execute () {
		CameraOverheadControl.SetCamFocusPoint (point);
	}
}
