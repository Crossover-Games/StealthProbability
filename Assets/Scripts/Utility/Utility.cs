using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static utility methods.
/// </summary>
public static class Utility {
	/// <summary>
	/// Returns an opaque color with RGB components specified as the XYZ components of the vector.
	/// </summary>
	public static Color ColorFromVector (Vector3 vector) {
		return new Color (vector.x, vector.y, vector.z);
	}

	/// <summary>
	/// Returns an color with RGB components specified as the XYZ components of the vector and alpha component given.
	/// </summary>
	public static Color ColorFromVector (Vector3 vector, float alpha) {
		return new Color (vector.x, vector.y, vector.z, alpha);
	}

	/// <summary>
	/// Max value is divided into a number of segments. This returns the index of the segment value falls into. Zero based
	/// </summary>
	public static int Partition (float value, float max, int segments) {
		for (int x = 0; x < segments; x++) {
			if (value < max * (x + 1) / segments) {
				return x;
			}
		}
		return 0;
	}
}
