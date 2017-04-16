using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Print the object's position on a key press.
/// </summary>
public class PrintPosition : MonoBehaviour {

	[SerializeField] private KeyCode activator;

	private void PositionPrint () {
		print (transform.position);
	}
	void Update () {
		if (Input.GetKeyDown (activator)) {
			PositionPrint ();
		}
	}
}
