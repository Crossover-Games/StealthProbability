using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStage : MonoBehaviour {

	[TooltipAttribute ("Do not modify")]
	[SerializeField]
	private Transform subController;

	[SerializeField] private float tilt = 0f;
	[SerializeField] private float elevation = 1f;
	[SerializeField] private float lateralDistance = 5f;
	[SerializeField] private float rotationCycleDuration = 10f;
	private float currentTime = 0f;

	[SerializeField] private Transform target;
	private Transform prevTarget = null;

	private float ratio {
		get {
			return currentTime / rotationCycleDuration;
		}
	}

	void Update () {
		if (target != prevTarget && target != null) {
			prevTarget = target;
			transform.position = target.position;
		}

		subController.localPosition = new Vector3 (0f, elevation, -lateralDistance);

		transform.rotation = Quaternion.Euler (tilt, 360f * ratio, 0f);

		currentTime += Time.deltaTime;
	}
}
