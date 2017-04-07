using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static methods for various types of interpolation. Works for vectors, floats, quaternions, and colors.
/// </summary>
public static class Interpolation {

	// ---FLOAT INTERPOLATION

	/*

	/// <summary>
	/// Greatest change with t close to 1. Interpolates fraction t of the way from a to b along a quadratic curve. t is clamped between 0 and 1.
	/// </summary>
	public static float Quaderp (float a, float b, float t) {
		float tPrime = Mathf.Clamp01 (t);
		tPrime *= tPrime;
		return InterpolationHelper (a, b, tPrime);
	}

	/// <summary>
	/// Greatest change with t close to 0. Interpolates fraction t of the way from a to b along a square root curve. t is clamped between 0 and 1.
	/// </summary>
	public static float Sqrterp (float a, float b, float t) {
		float tPrime = Mathf.Sqrt (Mathf.Clamp01 (t));
		return InterpolationHelper (a, b, tPrime);
	}

	/// <summary>
	/// Greatest change with t close to 0.5. Interpolates fraction t of the way from a to b along a sinusoidal curve. t is clamped between 0 and 1.
	/// </summary>
	public static float Sinerp (float a, float b, float t) {
		float tPrime = Mathf.Clamp01 (t) * Mathf.PI;
		tPrime = (1f - Mathf.Cos (tPrime)) / 2;
		return InterpolationHelper (a, b, tPrime);
	}

	/// <summary>
	/// Least change with t close to 0.5. Interpolates fraction t of the way from a to b along a cubic curve. t is clamped between 0 and 1.
	/// </summary>
	public static float Cuberp (float a, float b, float t) {
		float tPrime = Mathf.Clamp01 (t);
		tPrime = (2 * tPrime - 1);
		tPrime = tPrime * tPrime * tPrime + 1;
		tPrime = tPrime / 2;
		return InterpolationHelper (a, b, tPrime);
	}

	// ---VECTOR3 INTERPOLATION

	/// <summary>
	/// Greatest change with t close to 1. Interpolates fraction t of the way from a to b along a quadratic curve. t is clamped between 0 and 1.
	/// </summary>
	public static Vector3 Quaderp (Vector3 a, Vector3 b, float t) {
		return new Vector3 (Quaderp (a.x, b.x, t), Quaderp (a.y, b.y, t), Quaderp (a.z, b.z, t));
	}

	/// <summary>
	/// Greatest change with t close to 0. Interpolates fraction t of the way from a to b along a square root curve. t is clamped between 0 and 1.
	/// </summary>
	public static Vector3 Sqrterp (Vector3 a, Vector3 b, float t) {
		return new Vector3 (Sqrterp (a.x, b.x, t), Sqrterp (a.y, b.y, t), Sqrterp (a.z, b.z, t));
	}

	/// <summary>
	/// Greatest change with t close to 0.5. Interpolates fraction t of the way from a to b along a sinusoidal curve. t is clamped between 0 and 1.
	/// </summary>
	public static Vector3 Sinerp (Vector3 a, Vector3 b, float t) {
		return new Vector3 (Sinerp (a.x, b.x, t), Sinerp (a.y, b.y, t), Sinerp (a.z, b.z, t));
	}

	/// <summary>
	/// Least change with t close to 0.5. Interpolates fraction t of the way from a to b along a cubic curve. t is clamped between 0 and 1.
	/// </summary>
	public static Vector3 Cuberp (Vector3 a, Vector3 b, float t) {
		return new Vector3 (Cuberp (a.x, b.x, t), Cuberp (a.y, b.y, t), Cuberp (a.z, b.z, t));
	}
	*/

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
