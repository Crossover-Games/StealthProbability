using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This object will always be a fixed distance from its target transform.
/// </summary>
public class TransformLink : MonoBehaviour {

	private Transform myTarget = null;
	private Vector3 distance;

	/// <summary>
	/// Setting this sets the target and establishes the distance. Set it to null to disable.
	/// </summary>
	/// <value>The target.</value>
	public Transform target {
		get { return myTarget; }
		set {
			myTarget = value;
			if (myTarget != null) {
				distance = transform.position - myTarget.position;
			}
		}
	}

	void FixedUpdate () {
		if (myTarget != null) {
			transform.position = myTarget.position + distance;
		}
	}
}
