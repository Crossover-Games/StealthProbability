using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyEscape : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			UnityEditor.EditorApplication.isPlaying = false;
		}
	}
}
