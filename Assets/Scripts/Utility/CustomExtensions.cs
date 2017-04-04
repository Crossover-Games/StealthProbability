using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// Helpful extensions written by the king rat himself.
/// </summary>
public static class CustomExtensions {

	/// <summary>
	/// Moves the character to an absolute location in world space. Velocity is calculated normally.
	/// </summary>
	public static void AbsoluteMove (this CharacterController character, Vector3 destination) {
		character.Move (destination - character.transform.position);
	}

	/// <summary>
	/// Used in the level builder.
	/// </summary>
	public static GameObject ToStringCustom (this Compass.Direction myself) {
		switch (myself) {
			case Compass.Direction.North:
				return "North";
			case Compass.Direction.South:
				return "South";
			case Compass.Direction.East:
				return "East";
			case Compass.Direction.West:
				return "West";
			default:
				return "Invalid";
		}
	}

	/// <summary>
	/// Destroys all immediate children of this object. Don't call this in game.
	/// </summary>
	public static GameObject DestroyAllChildren (this GameObject myself) {
		foreach (Transform child in myself.transform) {
			GameObject.DestroyImmediate (child.gameObject);
		}
		return null;
	}

	// ---HASHSET
	
	/// <summary>
	/// Returns this set as an array in no particular order.
	/// </summary>
	public static T[] ToArray<T> (this HashSet<T> theSet) {
		T[] tempArray = new T[theSet.Count];
		theSet.CopyTo (tempArray);
		return tempArray;
	}

	/// <summary>
	/// Returns this set as a list in no particular order.
	/// </summary>
	public static List<T> ToList<T> (this HashSet<T> theSet) {
		List<T> tmp = new List<T> ();
		foreach (T t in theSet) {
			tmp.Add (t);
		}
		return tmp;
	}

	/// <summary>
	/// Create a shallow copy of this set.
	/// </summary>
	public static HashSet<T> Clone<T> (this HashSet<T> theSet) {
		HashSet<T> tmp = new HashSet<T> ();
		foreach (T t in theSet) {
			tmp.Add (t);
		}
		return tmp;
	}

	// ---ICOLLECTION

	/// <summary>
	/// Returns a random element in the collection.
	/// </summary>
	public static T RandomElement<T> (this ICollection<T> collection) {
		int current = 0;
		int magicIndex = UnityEngine.Random.Range (0, collection.Count);
		foreach (T t in collection) {
			if (current == magicIndex) {
				return t;
			}
			current++;
		}
		//this shouldn't fire, but who knows
		return default(T);
	}

	// ---LIST

	/// <summary>
	/// Returns the last element in the list.
	/// </summary>
	public static T LastElement<T> (this List<T> theList) {
		return theList [theList.Count - 1];
	}

	/// <summary>
	/// Create a shallow copy of this list.
	/// </summary>
	public static List<T> Clone<T> (this List<T> theList) {
		List<T> tmp = new List<T> ();
		for (int x = 0; x < theList.Count; x++) {
			tmp.Add (theList [x]);
		}
		return tmp;
	}

	/// <summary>
	/// Create a new list from startIndex to endIndex, inclusive. 
	/// </summary>
	public static List<T> Subset<T> (this List<T> theList, int startIndex, int endIndex) {
		if (startIndex > endIndex) {
			return null;
		}
		else {
			int newStart = Mathf.Max (startIndex, 0);
			int newEnd = Mathf.Min (endIndex, theList.Count - 1);
			List<T> tmp = new List<T> ();
			for (int x = newStart; x <= newEnd; x++) {
				tmp.Add (theList [x]);
			}
			return tmp;
		}
	}

	/// <summary>
	/// Create a new list from startIndex (inclusive) to the end of the list. There is an overload with the end index.
	/// </summary>
	public static List<T> Subset<T> (this List<T> theList, int startIndex) {
		return Subset (theList, startIndex, theList.Count - 1);
	}

	/// <summary>
	/// Randomly reorders this list.
	/// </summary>
	public static void Shuffle<T> (this List<T> list) {
		RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider ();
		int n = list.Count;
		while (n > 1) {
			byte[] box = new byte[1];
			do {
				provider.GetBytes (box);
			} while (!(box [0] < n * (Byte.MaxValue / n)));
			int k = (box [0] % n);
			n--;
			T value = list [k];
			list [k] = list [n];
			list [n] = value;
		}
	}

	// ---VECTOR3

	/// <summary>
	/// Returns the midpoint between this vector and the other.
	/// </summary>
	public static Vector3 Halfway (this Vector3 vector, Vector3 other) {
		return Vector3.Lerp (vector, other, 0.5f);
	}

	// ---COLOR

	/// <summary>
	/// Returns the RGB components of the color in a vector.
	/// </summary>
	public static Vector3 ToRGBVector (this Color color) {
		return new Vector3 (color.r, color.g, color.b);
	}
}
