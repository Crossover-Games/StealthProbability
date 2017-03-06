using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchThing : MonoBehaviour {

	public GameObject body;
	public Vector3 target;

	void Update () {
		Vector3 tmp = new Vector3 (1f, 1f, Vector3.Distance (transform.position, target));
		body.transform.localScale = tmp;
		transform.LookAt (target);
	}

	public void ImmediateHide () {
		body.transform.localScale = Vector3.zero;
	}
}
