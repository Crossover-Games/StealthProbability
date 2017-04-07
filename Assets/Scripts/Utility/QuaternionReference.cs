using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapper class for quaternion.
/// </summary>
public class QuaternionReference {
	/// <summary>
	/// The quaternion.
	/// </summary>
	public Quaternion quaternion;

	public QuaternionReference (Quaternion q) {
		quaternion = q;
	}
}
