using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 2D array of values that can be serialized when extended to a specific type.
/// </summary>
[Serializable]
public class FlatArray2D<T> : IEnumerable<T> {
	[SerializeField] private T [] array = new T [0];

	/// <summary>
	/// Total number of elements in the structure.
	/// </summary>
	public int Length {
		get { return array.Length; }
	}

	[SerializeField] private int rowLength = 0;

	/// <summary>
	/// Gets the length of the grid in a specified dimensions, represented by the order of the index.
	/// </summary>
	public int GetLength (int dimension) {
		if (dimension == 0) {
			return rowLength;
		}
		else if (dimension == 1) {
			return Length / rowLength;
		}
		else {
			throw new ArgumentException ("This structure only has dimensions 0 and 1.");
		}
	}

	public T this [int index0, int index1] {
		get { return array [index1 * rowLength + index0]; }
		set { array [index1 * rowLength + index0] = value; }
	}

	/// <summary>
	/// Returns a shallow copy of this as a genuine rectangular array.
	/// </summary>
	public T [,] Get2DShallow () {
		return array.Unflattened (rowLength);
	}

	/// <summary>
	/// Sets this structure to be effectively equivalent to the specified array. 
	/// </summary>
	public void Set2DShallow (T [,] newValue) {
		array = newValue.Flattened ();
		rowLength = newValue.GetLength (0);
	}

	public IEnumerator<T> GetEnumerator () {
		foreach (T t in array) {
			yield return t;
		}
	}

	IEnumerator IEnumerable.GetEnumerator () {
		return GetEnumerator ();
	}
}
