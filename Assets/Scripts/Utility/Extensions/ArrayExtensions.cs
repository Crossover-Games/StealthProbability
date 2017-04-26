using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extension methods for arrays of one and two dimensions.
/// </summary>
public static class ArrayExtension {
	/// <summary>
	/// True if the array contains the specified element.
	/// </summary>
	public static bool Contains<T> (this T [] array, T element) {
		return Array.IndexOf (array, element) > -1;
	}

	/// <summary>
	/// Returns a random element in the array.
	/// </summary>
	public static T RandomElement<T> (this T [] collection) {
		int current = 0;
		int magicIndex = UnityEngine.Random.Range (0, collection.Length);
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
	/// Returns a copy of the array with a row inserted before index 0 of dimension 0.
	/// </summary>
	public static T [,] RowInsertedAtZero<T> (this T [,] array, T defaultValue = default (T)) {
		T [,] arrayNew = new T [array.GetLength (0), array.GetLength (1) + 1];
		for (int i = 0; i < arrayNew.GetLength (0); i++) {
			for (int j = arrayNew.GetLength (1) - 1; j > 0; j--) {
				arrayNew [i, j] = array [i, j - 1];
			}
			arrayNew [i, 0] = defaultValue;
		}
		return arrayNew;
	}

	/// <summary>
	/// Returns a copy of the array with the row at index 0 of dimension 0 removed.
	/// </summary>
	public static T [,] RowRemovedAtZero<T> (this T [,] array) {
		T [,] arrayNew = new T [array.GetLength (0), array.GetLength (1) - 1];
		for (int i = 0; i < arrayNew.GetLength (0); i++) {
			for (int j = 0; j < arrayNew.GetLength (1); j++) {
				arrayNew [i, j] = array [i, j + 1];
			}
		}
		return arrayNew;
	}

	/// <summary>
	/// Returns a copy of the array with a column inserted before index 0 of dimension 1.
	/// </summary>
	public static T [,] ColumnInsertedAtZero<T> (this T [,] array, T defaultValue = default (T)) {
		T [,] arrayNew = new T [array.GetLength (0) + 1, array.GetLength (1)];
		for (int j = 0; j < arrayNew.GetLength (1); j++) {
			for (int i = arrayNew.GetLength (0) - 1; i > 0; i--) {
				arrayNew [i, j] = array [i - 1, j];
			}
			arrayNew [0, j] = defaultValue;
		}
		return arrayNew;
	}

	/// <summary>
	/// Returns a copy of the array with the row at index 0 of dimension 0 removed.
	/// </summary>
	public static T [,] ColumnRemovedAtZero<T> (this T [,] array) {
		T [,] arrayNew = new T [array.GetLength (0) - 1, array.GetLength (1)];
		for (int i = 0; i < arrayNew.GetLength (0); i++) {
			for (int j = 0; j < arrayNew.GetLength (1); j++) {
				arrayNew [i, j] = array [i + 1, j];
			}
		}
		return arrayNew;
	}
}
