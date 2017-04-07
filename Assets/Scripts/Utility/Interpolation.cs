using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static methods for various types of interpolation. Works for vectors, floats, quaternions, and colors.
/// </summary>
public static class Interpolation {

	/// <summary>
	/// Applies the appropriate transformation to the t parameter based on the interpolation method.
	/// </summary>
	private static float TimeScale (float t, InterpolationMethod method) {
		float tPrime = Mathf.Clamp01 (t);
		switch (method) {
			case InterpolationMethod.Linear:
				return tPrime;
			case InterpolationMethod.Quadratic:
				return tPrime * tPrime;
			case InterpolationMethod.SquareRoot:
				return Mathf.Sqrt (Mathf.Clamp01 (t));
			case InterpolationMethod.Sinusoidal:
				tPrime *= Mathf.PI;
				return (1f - Mathf.Cos (tPrime)) / 2;
			case InterpolationMethod.Cubic:
				tPrime = (2 * tPrime - 1);
				tPrime = tPrime * tPrime * tPrime + 1;
				return tPrime / 2;
			default:
				return tPrime;
		}
	}

	/// <summary>
	/// Interpolate between 2 floats with a specified interpolation method.
	/// </summary>
	public static float Interpolate (float a, float b, float t, InterpolationMethod method) {
		return Mathf.Lerp (a, b, TimeScale (t, method));
	}

	/// <summary>
	/// Interpolate between 2 vectors with a specified interpolation method.
	/// </summary>
	public static Vector3 Interpolate (Vector3 a, Vector3 b, float t, InterpolationMethod method) {
		return Vector3.Lerp (a, b, TimeScale (t, method));
	}

	/// <summary>
	/// Interpolate between 2 quaternions with a specified interpolation method.
	/// </summary>
	public static Quaternion Interpolate (Quaternion a, Quaternion b, float t, InterpolationMethod method) {
		return Quaternion.Lerp (a, b, TimeScale (t, method));
	}

	/// <summary>
	/// Interpolate between 2 colors with a specified interpolation method.
	/// </summary>
	public static Color Interpolate (Color a, Color b, float t, InterpolationMethod method) {
		return Color.Lerp (a, b, TimeScale (t, method));
	}

	/// <summary>
	/// The basic interpolation formula. Ideally, t will be skewed in different ways depending on the shape of the interpolation function.
	/// </summary>
	private static float InterpolationHelper (float a, float b, float t) {
		return a + t * (b - a);
	}
}
