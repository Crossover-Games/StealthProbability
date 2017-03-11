using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helpful extensions written by the king rat himself.
/// </summary>
public static class CustomExtensions {

	// HASHSET

	/// <summary>
	/// Moves the character to an absolute location in world space. Velocity is calculated normally.
	/// </summary>
	public static void AbsoluteMove (this CharacterController character, Vector3 destination) {
		character.Move (destination - character.transform.position);
	}

	/// <summary>
	/// Returns this set as an array in no particular order.
	/// </summary>
	public static T[] ToArray<T> (this HashSet<T> theSet) {
		T[] tempArray = new T[theSet.Count];
		theSet.CopyTo (tempArray);
		return tempArray;
	}

	/// <summary>
	/// Returns a random element in the collection.
	/// </summary>
	public static T RandomElement<T> (this ICollection<T> collection) {
		int current = 0;
		int magicIndex = Random.Range (0, collection.Count);
		foreach (T t in collection) {
			if (current == magicIndex) {
				return t;
			}
			current++;
		}
		//this shouldn't fire, but who knows
		return default(T);
	}

	// LIST

	/// <summary>
	/// Returns the last element in the list.
	/// </summary>
	public static T LastElement<T> (this List<T> theList) {
		return theList [theList.Count - 1];
	}

	// VECTOR3

	/// <summary>
	/// Returns the midpoint between this vector and the other.
	/// </summary>
	public static Vector3 Halfway (this Vector3 vector, Vector3 other) {
		return Vector3.Lerp (vector, other, 0.5f);
	}

	/// FAKE

	public static string DileepLovesPoop (this string poopoodilu) {
		return poopoodilu + " because dileep loves poop lol";
	}
}
