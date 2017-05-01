using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will be attached to the camera guide.
/// </summary>
public class DragCameraFollowPoint : MonoBehaviour {

	[SerializeField] private float followSpeed;

	[SerializeField] private Transform cameraTransform;

	void Update () {
		if (Input.GetMouseButtonUp (1)) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else if (Input.GetMouseButtonDown (1)) {
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		if (Input.GetMouseButton (1)) {
			/*Vector3 tmp = transform.position;
			tmp += new Vector3 (cameraTransform.transform.forward.x, 0, cameraTransform.transform.forward.z).normalized * -Input.GetAxis ("Mouse Y") * followSpeed * Time.deltaTime;
			tmp += cameraTransform.right * -Input.GetAxis ("Mouse X") * followSpeed * Time.deltaTime;
			transform.position = tmp;*/
			MoveCameraTarget (new Vector2 (-Input.GetAxis ("Mouse X"), -Input.GetAxis ("Mouse Y")));
		}
		Vector2 axes = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
		if (axes.magnitude > 0.01f) {
			MoveCameraTarget (axes);
		}
	}

	private void MoveCameraTarget (Vector2 movement) {
		Vector3 tmp = transform.position;
		tmp += new Vector3 (cameraTransform.transform.forward.x, 0, cameraTransform.transform.forward.z).normalized * movement.y * followSpeed * Time.deltaTime;
		tmp += cameraTransform.right * movement.x * followSpeed * Time.deltaTime;
		transform.position = tmp;
	}
}