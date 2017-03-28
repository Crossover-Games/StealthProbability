using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// what a hack
/// </summary>
public class Unfreeze2DRigidbodies : MonoBehaviour {

	/// <summary>
	/// hack
	/// </summary>
	public void Unfreeze () {
		foreach (Rigidbody2D rb2 in GetComponentsInChildren<Rigidbody2D>()) {
			rb2.bodyType = RigidbodyType2D.Dynamic;
		}
	}
}
