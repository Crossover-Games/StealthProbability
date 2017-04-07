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
}
