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
	public Vector3? position;

	/// <summary>
	/// Destination rotation.
	/// </summary>
	public Quaternion? rotation;

	/// <summary>
	/// Destination local scale.
	/// </summary>
	public Vector3? localScale;

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
	public AnimationDestination (Vector3? position, Quaternion? rotation, Vector3? localScale, float duration, InterpolationMethod interpolationMethod) {
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
		position = t.position;
		rotation = t.rotation;
		localScale = t.localScale;
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
