﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


/// <summary>
/// Helpful extensions written by the king rat himself.
/// </summary>
public static class MiscExtensions {


	// --GAMEOBJECT

	/// <summary>
	/// Moves the object and all children to the "Ignore Raycast" layer
	/// </summary>
	public static void MoveToIgnoreRaycastLayer (this GameObject go) {
		int ignoreRaycastLayer = LayerMask.NameToLayer ("Ignore Raycast");
		foreach (Transform t in go.GetComponentsInChildren<Transform> ()) {
			t.gameObject.layer = ignoreRaycastLayer;
		}
	}

	/// <summary>
	/// Functions like GameObject.GetComponentInChildren, but also searches inactive objects
	/// </summary>
	public static T GetComponentInChildrenUnconditional<T> (this GameObject go) {
		T component = go.GetComponent<T> ();

		if (component != null) {
			Transform root = go.transform;
			int numChildren = root.childCount;
			for (int childIndex = 0; childIndex < numChildren; childIndex++) {
				component = root.GetChild (childIndex).gameObject.GetComponentInChildrenUnconditional<T> ();
				if (component != null) {
					return component;
				}
			}
		}

		return component;
	}

	/// <summary>
	/// Moves the character to an absolute location in world space. Velocity is calculated normally.
	/// </summary>
	public static void AbsoluteMove (this CharacterController character, Vector3 destination) {
		character.Move (destination - character.transform.position);
	}

	/// <summary>
	/// Used in the level builder.
	/// </summary>
	public static string ToStringCustom (this Compass.Direction myself) {
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
	public static T [] ToArray<T> (this HashSet<T> theSet) {
		T [] tempArray = new T [theSet.Count];
		theSet.CopyTo (tempArray);
		return tempArray;
	}

	/// <summary>
	/// Returns this enumerable as a list in no particular order.
	/// </summary>
	public static List<T> ToList<T> (this IEnumerable<T> theSet) {
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
	/// Returns the first element in the collection, as dictated by iteration.
	/// </summary>
	public static T FirstElement<T> (this ICollection<T> collection) {
		foreach (T t in collection) {
			return t;
		}
		return default (T);
	}

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
		return default (T);
	}

	/// <summary>
	/// Enqueues a whole collection of items.
	/// </summary>
	public static void EnqueueRange<T> (this Queue<T> q, IEnumerable<T> addThese) {
		foreach (T t in addThese) {
			q.Enqueue (t);
		}
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
			byte [] box = new byte [1];
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
	public static Vector3 HalfwayTo (this Vector3 vector, Vector3 other) {
		return Vector3.Lerp (vector, other, 0.5f);
	}

	/// <summary>
	/// Arithmetic mean of the x, y, and z components of the vector.
	/// </summary>
	public static float Average (this Vector3 vector) {
		return (vector.x + vector.y + vector.z) / 3f;
	}

	// ---COLOR

	/// <summary>
	/// Returns the RGB components of the color in a vector.
	/// </summary>
	public static Vector3 ToRGBVector (this Color color) {
		return new Vector3 (color.r, color.g, color.b);
	}

	/// <summary>
	/// Returns a copy of the color with full value and half saturation.
	/// </summary>
	public static Color OptimizedForText (this Color color) {
		float h, s, v;
		Color.RGBToHSV (color, out h, out s, out v);
		return Color.HSVToRGB (h, s / 2, 1f);
	}

	/// <summary>
	/// Returns a copy of the color a specified alpha value.
	/// </summary>
	public static Color AlphaDifferent (this Color color, float alpha) {
		return new Color (color.r, color.g, color.b, alpha);
	}

	// ---RENDERER

	/// <summary>
	/// Changes the main material color. This will replace the material with a clone instance.
	/// </summary>
	public static void SetMainMaterialColor (this Renderer renderer, Color color) {
		Material m = renderer.material;
		m.color = color;
		renderer.material = m;
	}
}
