using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static methods for various types of interpolation. Works for vectors and floats. May add colors later. 
/// </summary>
public static class Interpolation {

	// FLOAT INTERPOLATION

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

	// VECTOR3 INTERPOLATION

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

	/// <summary>
	/// The basic interpolation formula. Ideally, t will be skewed in different ways depending on the shape of the interpolation function.
	/// </summary>
	private static float InterpolationHelper (float a, float b, float t) {
		return a + t * (b - a);
	}
}
