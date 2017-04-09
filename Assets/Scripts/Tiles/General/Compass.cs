using UnityEngine;

/// <summary>
/// Contains the cardinal directions and related methods.
/// </summary>
public class Compass {
	public enum Direction {
		North,
		South,
		East,
		West
	}

	/// <summary>
	/// Given a cardinal direction, return an absolute rotation for a character facing that direction.
	/// </summary>
	public static Quaternion DirectionToRotation (Direction d) {
		return Quaternion.Euler (new Vector3 (0f, DirectionToEulerY (d), 0f));
	}

	private static float DirectionToEulerY (Direction d) {
		switch (d) {
			case Direction.North:
				return 0f;
			case Direction.South:
				return 180f;
			case Direction.East:
				return 90f;
			case Direction.West:
				return 270f;
			default:
				return 0f;	// failsafe, should just point it north
		}
	}

	private static float NormalizeEulerCoordinate (float f) {
		if (f > 180f) {
			return f - 360f;
		}
		else {
			return f;
		}
	}

	public static Direction[] allDirections {
		get {
			Direction[] allDir = {
				Direction.North,
				Direction.South,
				Direction.East,
				Direction.West
			}; 
			return allDir;
		}
	}

	public static Direction oppositeDirection (Direction direction) {
		switch (direction) {
			case Direction.North:
				return Direction.South;
			case Direction.South:
				return Direction.North;
			case Direction.East:
				return Direction.West;
			case Direction.West:
				return Direction.East;
			default:
				return Direction.North;
		}
	}

	/// <summary>
	/// Interpolates betweens directions a and b by t, and returns this as a Quaternion rotation.
	/// </summary>
	public static Quaternion CompassRotationLerp (Direction a, Direction b, float t) {
		float aY = DirectionToEulerY (a);
		float bY = DirectionToEulerY (b);
		if (Mathf.Abs (aY - bY) > 180f) {
			aY = NormalizeEulerCoordinate (aY);
			bY = NormalizeEulerCoordinate (bY);
		}

		return Quaternion.Euler (new Vector3 (0f, Mathf.Lerp (aY, bY, t), 0f));
		//return Quaternion.Euler (new Vector3 (0f, Interpolation.Sqrterp (aY, bY, t), 0f));
	}
}
