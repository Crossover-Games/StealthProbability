using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will be attached to the camera guide.
/// </summary>
public class DragCameraFollowPoint : MonoBehaviour {

	[SerializeField] private float followSpeed;

	[Tooltip ("Assign the main camera to this.")]
	[SerializeField] private Transform cameraTransform;

	void Update () {
		if (Input.GetMouseButtonUp (1) || Input.GetMouseButtonUp (2) || Input.GetMouseButtonDown (1) || Input.GetMouseButtonDown (2)) {
			if (Input.GetMouseButton (1) || Input.GetMouseButton (2)) {
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
			else {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		if (Input.GetMouseButton (1)) {
			Vector3 tmp = transform.position;
			tmp += new Vector3 (cameraTransform.transform.forward.x, 0, cameraTransform.transform.forward.z).normalized * -Input.GetAxis ("Mouse Y") * followSpeed * Time.deltaTime;
			tmp += cameraTransform.right * -Input.GetAxis ("Mouse X") * followSpeed * Time.deltaTime;

			transform.position = tmp;
		}
		if (Input.GetMouseButton (2)) {
			Vector3 euler = cameraTransform.eulerAngles;
			euler.y += Input.GetAxis ("Mouse X") * followSpeed * 12 * Time.deltaTime;
			cameraTransform.eulerAngles = euler;
		}
	}
}