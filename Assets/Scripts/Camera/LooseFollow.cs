﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The object this component is attached to will gravitate toward the position of the target transform.
/// </summary>
public class LooseFollow : MonoBehaviour {
	/// <summary>
	/// The object will move toward this position. Set to null to disable following.
	/// </summary>
	[HideInInspector] public Transform target;

	/// <summary>
	/// faster at high numbers, none at zero 
	/// </summary>
	[SerializeField] private float followSpeed = 10f;

	void LateUpdate () {
		if (target != null) {
			transform.position = Vector3.Lerp (transform.position, target.position, Time.deltaTime * followSpeed);
		}
	}

	/// <summary>
	/// Immediately advance to the target.
	/// </summary>
	public void JumpToEnd () {
		transform.position = target.position;
	}
}
