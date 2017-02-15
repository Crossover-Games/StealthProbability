using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCharacterExample : GameCharacter {
	[SerializeField] private CharacterController myCharacterController;
	[SerializeField] private Animator myAnimator;

	private UniversalTileManager theManager;

	private float totalTime = 2f;
	private float elapsedTime = Mathf.Infinity;

	private Animator anim;

	void Awake () {
		theManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<UniversalTileManager> ();
		anim = GetComponentInChildren<Animator> ();
	}

	void Update () {
		if (elapsedTime < totalTime) {
			elapsedTime += Time.deltaTime;
		}
	}

	void FixedUpdate () {
		if (Input.GetKey (KeyCode.Space)) {
			//elapsedTime = 0f;
		}

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



		if (elapsedTime < totalTime) {
			myCharacterController.Move (Vector3.Lerp (transform.position, theManager.cursorTile.characterConnectionPoint, elapsedTime / totalTime) - transform.position);
		}

		myAnimator.SetFloat ("Speed", myCharacterController.velocity.magnitude);
	}

	//private void StepTo
}
