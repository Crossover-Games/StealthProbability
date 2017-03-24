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
		if (Input.GetMouseButton (1)) {
			Vector3 tmp = transform.position;
			tmp += new Vector3 (cameraTransform.transform.forward.x, 0, cameraTransform.transform.forward.z).normalized * -Input.GetAxis ("Mouse Y") * followSpeed * Time.deltaTime;
			tmp += cameraTransform.right * -Input.GetAxis ("Mouse X") * followSpeed * Time.deltaTime;

			transform.position = tmp;
		}
	}
}