using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to control the camera.
/// </summary>
public class CameraOverheadControl : MonoBehaviour {
	[Tooltip ("Assign the main camera to this.")]
	[SerializeField] private Transform cameraTransform;

	[Tooltip ("Assign the main camera to this.")]
	[SerializeField] private LooseFollow cameraContraption;

	[Tooltip ("Link to the pointer")]
	[SerializeField] private TransformLink pointer;

	[Tooltip ("Link to the pointer")]
	[SerializeField] private DragCameraFollowPoint dragControl;

	private int cameraPlaneLayer;

	void Awake () {
		cameraContraption.target = pointer.transform;
		cameraPlaneLayer = LayerMask.GetMask ("Camera Plane");
	}

	private Vector3 DirectionToCeiling {
		get {
			Vector3 tmp = cameraTransform.rotation * Vector3.forward;
			tmp *= -1;
			return tmp;
		}
	}

	/// <summary>
	/// The camera will focus on this static point. Use SetCamFollowTarget instead if you want to follow a moving object.
	/// </summary>
	public void SetCamFocusPoint (Vector3 location) {
		RaycastHit hit;

		// raycasts from the target on the floor upwards to the ceiling
		Physics.Raycast (location, DirectionToCeiling, out hit, Mathf.Infinity, cameraPlaneLayer);

		pointer.target = null;
		pointer.transform.position = hit.point;
	}

	/// <summary>
	/// The camera will follow this object. Use SetCamFocusPoint instead if you want to look at a static point.
	/// </summary>
	public void SetCamFollowTarget (Transform thing) {
		RaycastHit hit;

		// raycasts from the target on the floor upwards to the ceiling
		Physics.Raycast (thing.position, DirectionToCeiling, out hit, Mathf.Infinity, cameraPlaneLayer);

		pointer.transform.position = hit.point;
		pointer.target = thing;
	}

	/// <summary>
	/// The camera loses its association with the object it is tracking and stops at its current position.
	/// </summary>
	public void StopFollowing () {
		pointer.target = null;
	}

	/// <summary>
	/// Allows the player to drag the camera with the right mouse button.
	/// </summary>
	public bool dragControlAllowed {
		get { return dragControl.enabled; }
		set { dragControl.enabled = value; }
	}

}
