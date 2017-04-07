using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Like a Vector2, but with integer components.
/// </summary>
public struct Point2D {
	/// <summary>
	/// X coordinate.
	/// </summary>
	public int x;
	/// <summary>
	/// Y coordinate.
	/// </summary>
	public int y;

	public Point2D (int x, int y) {
		this.x = x;
		this.y = y;
	}

	public override bool Equals (object obj) {
		if (!(obj is Point2D)) {
			return false;
		}

		Point2D other = (Point2D)obj;
		return x == other.x && y == other.y;
	}

	public override int GetHashCode () {
		return x.GetHashCode () ^ y.GetHashCode ();
	}
	public static bool operator == (Point2D a, Point2D b) {
		return a.x == b.x && a.y == b.y;
	}
	public static bool operator != (Point2D a, Point2D b) {
		return !(a == b);
	}
}
