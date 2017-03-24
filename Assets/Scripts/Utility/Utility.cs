using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour {
	private static Vector2 previousMousePosition;
	public static Vector2 mouseDelta;

	void Update () {
		previousMousePosition = Input.mousePosition;
	}
}
