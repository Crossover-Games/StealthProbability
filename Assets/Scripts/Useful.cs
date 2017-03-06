using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class full of handy static methods.
/// </summary>
public class Useful {

	/// <summary>
	/// Converts a HashSet to an array in no particular order.
	/// </summary>
	public static T[] HashSetToArray<T> (HashSet<T> theSet) {
		T[] tempArray = new T[theSet.Count];
		theSet.CopyTo (tempArray);
		return tempArray;
	}

	/// <summary>
	/// Returns the only item in a hashset with a count of one.
	/// </summary>
	public static T GetHashSetSingleton<T> (HashSet<T> theSet) {
		if (theSet.Count == 1) {
			T tmp = default(T);
			foreach (T looper in theSet) {
				tmp = looper;
			}
			return tmp;
		}
		else {
			return default(T);
		}
	}
		
	public static T RandomElementInHashSet<T> (HashSet<T> theSet) {
		T[] tmp = HashSetToArray (theSet);
		return tmp [Random.Range (0, tmp.Length)];
	}
}
