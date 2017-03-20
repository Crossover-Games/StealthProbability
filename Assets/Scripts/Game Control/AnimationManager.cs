using UnityEngine;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour {
	private enum AnimationPhase {
		Rotating,
		Moving
	}
	private AnimationPhase phase = AnimationPhase.Rotating;

	private GameBrain brain;

	private float elapsedTime = 0f;

	/// <summary>
	/// The total time one step animation takes.
	/// </summary>
	private float animationTime;
	/// <summary>
	/// If the character rotates, this fraction of the time is spent rotating.
	/// </summary>
	private static float ROTATION_FRACTION = 0.33f;

	private float ROTATION_TIME {
		get { return animationTime * ROTATION_FRACTION; }
	}
	private float MOVE_TIME {
		get {
			if (startRotation == endRotation) {
				return animationTime;
			}
			else {
				return animationTime - ROTATION_TIME;
			}
		}
	}

	private Vector3 startPosition = Vector3.zero;
	private Vector3 endPosition = Vector3.zero;
	private Compass.Direction startRotation;
	private Compass.Direction endRotation;
	private GameCharacter character = null;

	/// <summary>
	/// If there is animation to be done, this is true.
	/// </summary>
	public bool activelyAnimating {
		get{ return character != null; }
	}

	/// <summary>
	/// Starts animating the character.
	/// </summary>
	public void StartAnimating (GameCharacter theCharacter, Vector3 destination, Compass.Direction targetOrientation) {
		if (character == null) {
			startPosition = theCharacter.transform.position;
			endPosition = destination;
			elapsedTime = 0f;

			startRotation = theCharacter.orientation;
			endRotation = targetOrientation;

			animationTime = theCharacter.stepAnimationTime;

			// no rotation needed
			if (startRotation == endRotation) {
				phase = AnimationPhase.Moving;
			}
			else {
				phase = AnimationPhase.Rotating;
				theCharacter.orientation = targetOrientation;
			}
				
			character = theCharacter;
		}
		else {
			print ("attempting to interrupt animation");
		}
	}
		
	/// <summary>
	/// Like MonoBehaviour.Update(), but only called while this thing is animating.
	/// </summary>
	public void AnimationUpdate () {
		switch (phase) {
			case AnimationPhase.Rotating:
				if (elapsedTime < ROTATION_TIME) {
					elapsedTime += Time.deltaTime;
					//character.transform.rotation = Quaternion.Slerp (startRotation, endRotation, elapsedTime / ROTATE_TIME);
					character.transform.rotation = Compass.CompassRotationLerp (startRotation, endRotation, elapsedTime / ROTATION_TIME);
				}
				else {
					elapsedTime = 0f;
					phase = AnimationPhase.Moving;
				}
				break;
			case AnimationPhase.Moving:
				if (elapsedTime < MOVE_TIME) {
					elapsedTime += Time.deltaTime;
					character.transform.position = Interpolation.Sinerp (startPosition, endPosition, elapsedTime / MOVE_TIME);
					//character.transform.position = Vector3.Lerp (startPosition, endPosition, elapsedTime / MOVE_TIME);
				}
				else {
					character = null;
				}
				break;
			default:
				character = null;
				break;
		}
	}
}
