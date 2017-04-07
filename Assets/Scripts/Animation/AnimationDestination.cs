using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A cluster of all target transform values in an animation. This includes position, rotation, and local scale.
/// </summary>
public class AnimationDestination {
	/// <summary>
	/// Destination world position.
	/// </summary>
	public Vector3Reference position;

	/// <summary>
	/// Destination rotation.
	/// </summary>
	public QuaternionReference rotation;

	/// <summary>
	/// Destination local scale.
	/// </summary>
	public Vector3Reference localScale;

	/// <summary>
	/// Animation duration.
	/// </summary>
	public float duration;

	/// <summary>
	/// The interpolation method used for the animation.
	/// </summary>
	public InterpolationMethod interpolationMethod;

	/// <summary>
	/// Builds an AnimationDestination from scratch. Leave any of the transform values null to keep them from animating.
	/// </summary>
	public AnimationDestination (Vector3Reference position, QuaternionReference rotation, Vector3Reference localScale, float duration, InterpolationMethod interpolationMethod) {
		this.position = position;
		this.rotation = rotation;
		this.localScale = localScale;
		this.duration = duration;
		this.interpolationMethod = interpolationMethod;
	}

	/// <summary>
	/// Builds a AnimationDestination from the values in an actual transform.
	/// </summary>
	public AnimationDestination (Transform t, float duration, InterpolationMethod interpolationMethod) {
		position = new Vector3Reference (t.position);
		rotation = new QuaternionReference (t.rotation);
		localScale = new Vector3Reference (t.localScale);
		this.duration = duration;
		this.interpolationMethod = interpolationMethod;
	}

	/// <summary>
	/// Builds an AnimationDestination from the values in an actual transform. Will not animate: only used as an initial state.
	/// </summary>
	public static AnimationDestination CreateOrigin (Transform t) {
		AnimationDestination tmp = new AnimationDestination (t, 0f, InterpolationMethod.Linear);
		return tmp;
	}
}
