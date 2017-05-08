using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionTile : Floor {
	public abstract bool active { get; }
	private OneShotSound sound;

	public abstract void Activate ();

	protected override void LateAwake () {
		sound = GetComponent<OneShotSound> ();
	}

	public void PlaySound () {
		sound.PlaySound ();
	}
}
