using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Target is enabled on Awake. Good for keeping annoying hacker things disabled in the editor.
/// </summary>
public class EnableTargetOnAwake : MonoBehaviour {
	public GameObject target;
	void Awake () {
		target.SetActive (true);
	}
}
