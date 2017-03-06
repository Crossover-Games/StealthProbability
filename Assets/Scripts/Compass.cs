using UnityEngine;

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
	public static Quaternion DirectionToRotation (Direction cd) {
		switch (cd) {
			case Direction.North:
				return Quaternion.Euler (new Vector3 (0f, 0f, 0f));
			case Direction.South:
				return Quaternion.Euler (new Vector3 (0f, 0f, 180f));
			case Direction.East:
				return Quaternion.Euler (new Vector3 (0f, 0f, 90f));
			case Direction.West:
				return Quaternion.Euler (new Vector3 (0f, 0f, 270f));
			default:
				return Quaternion.identity;	// failsafe, should just point it north
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
}
