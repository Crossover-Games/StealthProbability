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

	[Tooltip ("This is just some dummy thing to guide the camera along the invisible ceiling.")]
	[SerializeField] private TransformLink pointer;

	private int cameraPlaneLayer;

	void Awake () {
		cameraPlaneLayer = LayerMask.GetMask ("Camera Plane");
	}

	/// <summary>
	/// Tell the camera to stop moving. Ideally, there shouldn't be any reason to call this.
	/// </summary>
	public void DisableCameraFollow () {
		cameraContraption.target = null;
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
		cameraContraption.target = pointer.transform;
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
		cameraContraption.target = pointer.transform;
	}

}
