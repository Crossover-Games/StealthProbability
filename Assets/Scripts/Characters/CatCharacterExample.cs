using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCharacterExample : GameCharacter {
	[SerializeField] private CharacterController myCharacterController;
	[SerializeField] private Animator myAnimator;

	private Animator anim;

	override public CharacterType characterType {
		get{ return CharacterType.Cat; }
	}

	override protected void Awake () {
		base.Awake ();
		anim = GetComponentInChildren<Animator> ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			anim.Play ("RUN-DEMO");
		}
		else if (Input.GetKeyDown (KeyCode.O)) {
			anim.Play ("WALK-DEMO");
		}
		else if (Input.GetKeyDown (KeyCode.P)) {
			anim.Play ("Idle");
		}
		else if (Input.GetKeyDown (KeyCode.R)) {
			anim.Play ("roar");
		}
	}
}
