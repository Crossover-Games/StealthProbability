using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to control the camera.
/// </summary>
public class CameraOverheadControl : MonoBehaviour {
	[Tooltip ("Assign the main camera to this.")]
	[SerializeField] private Transform cameraRotationInstance;
	/// <summary>
	/// Transform that reads the pure rotation of the camera.
	/// </summary>
	private static Transform cameraRotation;

	[Tooltip ("Assign the main camera offset to this.")]
	[SerializeField] private LooseFollow cameraContraptionInstance;
	/// <summary>
	/// Loose follow control for the camera.
	/// </summary>
	private static LooseFollow cameraContraption;

	[Tooltip ("Link to the pointer")]
	[SerializeField] private TransformLink pointerInstance;
	/// <summary>
	/// The pointer's link to the current target.
	/// </summary>
	private static TransformLink pointer;

	[Tooltip ("Link to the pointer")]
	[SerializeField] private DragCameraFollowPoint dragControlInstance;
	/// <summary>
	/// Enables/disables user control of camera.
	/// </summary>
	private static DragCameraFollowPoint dragControl;

	/// <summary>
	/// Layer mask for the camera plane.
	/// </summary>
	private static int cameraPlaneLayer;

	/// <summary>
	/// Adjusts the target point to go for the real camera instead of the rotational offset.
	/// </summary>
	private static Vector3 cameraOffset {
		get {
			return cameraContraption.transform.position - cameraRotation.position;
		}
	}

	void Awake () {
		cameraContraptionInstance.target = pointerInstance.transform;
		cameraPlaneLayer = LayerMask.GetMask ("Camera Plane");

		cameraRotation = cameraRotationInstance;
		cameraContraption = cameraContraptionInstance;
		pointer = pointerInstance;
		dragControl = dragControlInstance;
	}

	private static Vector3 directionToCeiling {
		get {
			Vector3 tmp = cameraRotation.rotation * Vector3.forward;
			tmp *= -1;
			return tmp;
		}
	}

	/// <summary>
	/// The camera will focus on this static point. Use SetCamFollowTarget instead if you want to follow a moving object.
	/// </summary>
	public static void SetCamFocusPoint (Vector3 location) {
		RaycastHit hit;

		// raycasts from the target on the floor upwards to the ceiling
		Physics.Raycast (location, directionToCeiling, out hit, Mathf.Infinity, cameraPlaneLayer);

		pointer.target = null;
		pointer.transform.position = hit.point + cameraOffset;
	}

	/// <summary>
	/// The camera will follow this object. Use SetCamFocusPoint instead if you want to look at a static point.
	/// </summary>
	public static void SetCamFollowTarget (Transform thing) {
		RaycastHit hit;

		// raycasts from the target on the floor upwards to the ceiling
		Physics.Raycast (thing.position, directionToCeiling, out hit, Mathf.Infinity, cameraPlaneLayer);

		pointer.transform.position = hit.point + cameraOffset;
		pointer.target = thing;
	}

	/// <summary>
	/// The camera loses its association with the object it is tracking and stops at its current position.
	/// </summary>
	public static void StopFollowing () {
		pointer.target = null;
	}

	/// <summary>
	/// Allows the player to drag the camera with the right mouse button.
	/// </summary>
	public static bool dragControlAllowed {
		get { return dragControl.enabled; }
		set { dragControl.enabled = value; }
	}
}
