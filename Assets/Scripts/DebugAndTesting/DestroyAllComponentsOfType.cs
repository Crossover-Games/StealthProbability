using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllComponentsOfType : MonoBehaviour {

	[ContextMenu ("Destroy All Colliders")]
	public void DestroyAllColliders () {
		Collider [] c = FindObjectsOfType<Collider> ();
		for (int x = 0; x < c.Length; x++) {
			DestroyImmediate (c [x]);
		}
	}
}
