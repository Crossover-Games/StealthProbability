using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes an object bob and bounce.
/// </summary>
public class FloatingPowerup : MonoBehaviour {

	[Tooltip ("Length of one cycle in seconds. Do not set to zero")]
	[SerializeField]
	private float cycleLength = 1f;

	[Tooltip ("Max change from starting elevation")]
	[SerializeField]
	private float elevationDifference;
	[SerializeField] private bool reverseDirection = false;

	private float currentTime;

	private float defaultElevation;

	private float ratio {
		get {
			return currentTime / cycleLength;
		}
	}

	private float elevation {
		get {
			if (ratio < 0.5f) {
				return defaultElevation + Interpolation.Interpolate (elevationDifference, -elevationDifference, ratio * 2f, InterpolationMethod.Sinusoidal);
			}
			else {
				return defaultElevation + Interpolation.Interpolate (-elevationDifference, elevationDifference, (ratio - 0.5f) * 2f, InterpolationMethod.Sinusoidal);
			}
		}
	}

	void Awake () {
		currentTime = 0f;
		defaultElevation = transform.position.y;
		ApplyTransformation ();
	}

	void LateUpdate () {
		currentTime += Time.deltaTime;
		while (currentTime >= cycleLength) {
			currentTime -= cycleLength;
		}
		ApplyTransformation ();
	}

	private void ApplyTransformation () {
		Quaternion tempQuat = transform.rotation;
		Vector3 tempEuler = tempQuat.eulerAngles;
		float newY = reverseDirection ? -360f : 360f;
		newY *= ratio;
		tempEuler.y = newY;
		tempQuat.eulerAngles = tempEuler;
		transform.rotation = tempQuat;

		Vector3 tempPos = transform.position;
		tempPos.y = elevation;
		transform.position = tempPos;
	}
}
