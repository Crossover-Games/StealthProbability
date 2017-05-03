using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Timer that counts up to a given duration.
/// Call tick in a MonoBehaviour update.
/// </summary>
public class Timer {

	private float elapsedTime;

	private float myDuration;
	/// <summary>
	/// How long is this timer active? Does not change.
	/// </summary>
	public float duration {
		get { return myDuration; }
	}

	/// <summary>
	/// Creates a new timer of a specific duration. Starts with zero time elapsed.
	/// </summary>
	public Timer (float maxDuration) {
		elapsedTime = 0f;
		myDuration = maxDuration;
	}

	/// <summary>
	/// Forwards the timer. Call once in a MonoBehaviour.Update()
	/// </summary>
	public void Tick () {
		elapsedTime += Time.deltaTime;
	}

	/// <summary>
	/// Bring the timer back to its constructed max duration.
	/// </summary>
	public void Restart () {
		elapsedTime = 0f;
	}

	/// <summary>
	/// Ratio of completion this timer has run. 0 at start, 1 at completion. Duration 0 is always incomplete.
	/// </summary>
	public float ratio {
		get {
			if (myDuration == 0) {
				return 0f;
			}
			else {
				return Mathf.Clamp01 (elapsedTime / myDuration);
			}
		}
	}

	/// <summary>
	/// Advance the timer to the end prematurely.
	/// </summary>
	public void Disable () {
		elapsedTime = myDuration + 1f;
	}

	/// <summary>
	/// False when the timer is complete.
	/// </summary>
	public bool active {
		get { return elapsedTime <= myDuration; }
	}
}
