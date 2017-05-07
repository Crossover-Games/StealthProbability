using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour {

	[TooltipAttribute ("Do not modify")]
	[SerializeField]
	private Transform subController;

	[SerializeField] private float tilt = 0f;
	[SerializeField] private float maxElevation = 1f;
	[SerializeField] private float lateralDistance = 5f;
	[SerializeField] private float rotationCycleDuration = 10f;
	[SerializeField] private float elevationCycleDuration = 10f;
	private float rotationCurrentTime = 0f;
	private float elevationCurrentTime = 0f;

	[SerializeField] private InterpolationMethod elevationMethod;

	private float startingElevation;
	private bool currentlyGoingUp = true;

	private float rotationRatio {
		get {
			return rotationCurrentTime / rotationCycleDuration;
		}
	}
	private float elevationRatio {
		get {
			if (currentlyGoingUp) {
				return elevationCurrentTime / elevationCycleDuration;
			}
			else {
				return 1 - (elevationCurrentTime / elevationCycleDuration);
			}
		}
	}

	void Awake () {
		startingElevation = transform.position.y;
	}

	void Update () {
		subController.localPosition = new Vector3 (0f, Interpolation.Interpolate (startingElevation, startingElevation + maxElevation, elevationRatio, elevationMethod), -lateralDistance);
		transform.rotation = Quaternion.Euler (tilt, 360f * rotationRatio, 0f);
		rotationCurrentTime += Time.deltaTime;
		if (rotationCurrentTime > rotationCycleDuration) {
			rotationCurrentTime -= rotationCycleDuration;
		}
		elevationCurrentTime += Time.deltaTime;
		if (elevationCurrentTime > elevationCycleDuration) {
			elevationCurrentTime -= elevationCycleDuration;
			currentlyGoingUp = !currentlyGoingUp;
		}
	}
}
