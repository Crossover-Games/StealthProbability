using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scales time when the activator key is held.
/// </summary>
public class TimeScaleButtonHold : MonoBehaviour {

	[SerializeField] private KeyCode activator;
	[SerializeField] private float timeCoefficient;

	void Update () {
		if (Input.GetKeyUp (activator)) {
			Time.timeScale = 1f;
		}
		else if (Input.GetKeyDown (activator)) {
			Time.timeScale = timeCoefficient;
		}

	}
}
