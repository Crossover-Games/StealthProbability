using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Victory hack.
/// </summary>
public class VictoryHack : MonoBehaviour {

	public Transform cat;
	public Transform target;

	public GameBrain brain;

	public Sprite goodJob;
	public Unfreeze2DRigidbodies unfreezer;
	/// <summary>
	/// hack
	/// </summary>
	private void VictoryThing () {
		brain.JAMHACK ();
		foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>()) {
			spr.sprite = goodJob;
		}
		unfreezer.Unfreeze ();
	}

	void Update () {
		if (Vector3.Distance (cat.position, target.position) < 1f) {
			VictoryThing ();
		}
	}
}
