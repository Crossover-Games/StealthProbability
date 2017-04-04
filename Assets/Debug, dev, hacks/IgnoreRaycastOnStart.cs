using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a hack that should be included in the upcoming tile refactor
/// </summary>
public class IgnoreRaycastOnStart : MonoBehaviour {

	void Start () {
		foreach (Transform t in transform.GetComponentsInChildren<Transform>()) {
			t.gameObject.layer = LayerMask.NameToLayer ("Ignore Raycast");
		}
	}
}
