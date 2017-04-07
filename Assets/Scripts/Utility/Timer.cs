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
		get{ return myDuration; }
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
	/// Ratio of completion this timer has run. 0 at start, 1 at completion.
	/// </summary>
	public float ratio {
		get {
			if (myDuration == 0) {
				return 1f;
			}
			else {
				return Mathf.Clamp01 (elapsedTime / myDuration);
			}
		}
	}

	/// <summary>
	/// Cancel the timer.
	/// </summary>
	public void Disable () {
		elapsedTime = myDuration + 1;
	}

	/// <summary>
	/// False when the timer is complete.
	/// </summary>
	public bool active {
		get{ return elapsedTime <= myDuration; }
	}
}
