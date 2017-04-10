using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The object repeats itself on a path from its position on awake to the specified end location.
/// </summary>
public class CyclicMovement : MonoBehaviour {

	[SerializeField] private Vector3 destination;
	private Vector3 start;
	[Tooltip ("Time (seconds) to complete one cycle.")]
	[SerializeField]
	private float cycleDuration = 1f;

	[Tooltip ("Time (seconds) between cycles. The object will linger at its end position.")]
	[SerializeField]
	private float timeBetweenCycles = 0f;

	[SerializeField] private InterpolationMethod interpolationMethod;

	private Timer animTimer;
	private Timer restTimer;

	void Awake () {
		start = transform.position;
		animTimer = new Timer (cycleDuration);
		restTimer = new Timer (timeBetweenCycles);
	}
	void Update () {
		transform.position = Interpolation.Interpolate (start, destination, animTimer.ratio, interpolationMethod);
		animTimer.Tick ();
		if (!animTimer.active) {
			restTimer.Tick ();
			if (!restTimer.active) {
				restTimer.Restart ();
				animTimer.Restart ();
			}
		}
	}
}
