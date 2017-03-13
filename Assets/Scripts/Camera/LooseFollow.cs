using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The object this component is attached to will gravitate toward the position of the target transform.
/// </summary>
public class LooseFollow : MonoBehaviour {

	/// <summary>
	/// DUMMIED. Will be expanded upon when we start fleshing out the follow algorithm.
	/// </summary>
	private Vector3 previousPosition;
	private Transform targetTransform = null;
	/// <summary>
	/// The object will move toward this position. Set to null to disable following.
	/// </summary>
	public Transform target {
		get{ return targetTransform; }
		set {
			previousPosition = transform.position;
			targetTransform = value;
		}
	}
		
	/// <summary>
	/// faster at high numbers, none at zero 
	/// </summary>
	[SerializeField] private float followSpeed = 10f;

	void Awake () {
		previousPosition = transform.position;
	}

	void LateUpdate () {
		if (targetTransform != null) {
			transform.position = Vector3.Lerp (transform.position, targetTransform.position, Time.deltaTime * followSpeed);
		}
	}
}
