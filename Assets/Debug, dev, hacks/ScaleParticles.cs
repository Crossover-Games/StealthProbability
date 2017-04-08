using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Only intended to be run in the editor to fix the diddly darn tellyporter prefab
/// </summary>
public class ScaleParticles : MonoBehaviour {

	[Tooltip ("Multiplies the local scale and particle scale by this much")]
	public float scaleFactor = 1f;

	[ContextMenu ("Apply Scale")]
	void ApplyScale () {
		foreach (Transform t in GetComponentsInChildren<Transform>()) {
			t.localScale *= scaleFactor;
		}
		foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>()) {
			//ps.main.startSize = new ParticleSystem.MinMaxCurve ();
			ps.startSize *= scaleFactor;
		}
	}
}
