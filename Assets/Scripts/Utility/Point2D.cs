using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Like a Vector2, but with integer components.
/// </summary>
[System.Serializable]
public struct Point2D {
	/// <summary>
	/// X coordinate.
	/// </summary>
	public int x;
	/// <summary>
	/// Z coordinate.
	/// </summary>
	public int z;

	public Point2D (int x, int z) {
		this.x = x;
		this.z = z;
	}

	public override bool Equals (object obj) {
		if (!(obj is Point2D)) {
			return false;
		}

		Point2D other = (Point2D)obj;
		return x == other.x && z == other.z;
	}

	public override int GetHashCode () {
		return x.GetHashCode () ^ z.GetHashCode ();
	}
	public static bool operator == (Point2D a, Point2D b) {
		return a.x == b.x && a.z == b.z;
	}
	public static bool operator != (Point2D a, Point2D b) {
		return !(a == b);
	}

	/// <summary>
	/// Returns the XZ position of the transform as a Point2D.
	/// </summary>
	public static Point2D FromTransformXZ (Transform t) {
		return new Point2D (Mathf.RoundToInt (t.position.x), Mathf.RoundToInt (t.position.z));
	}

	/// <summary>
	/// Converts this point to a vector3 with Y = 0.
	/// </summary>
	public Vector3 ToVector3XZ () {
		return new Vector3 (x, 0, z);
	}

	/// <summary>
	/// Converts this point to a vector3 with specified y value.
	/// </summary>
	public Vector3 ToVector3XZ (float y) {
		return new Vector3 (x, y, z);
	}
}
