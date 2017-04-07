using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A single item to be animated multiple times. Created and disposed by AnimationManager.
/// </summary>
public class AnimationQueue {

	private Transform objectTransform;
	/// <summary>
	/// The object that this is animating.
	/// </summary>
	public Transform animatingObject {
		get { return objectTransform; }
	}

	/// <summary>
	/// The animating object's transform values at the start of an animation.
	/// </summary>
	private AnimationDestination initialValues;

	/// <summary>
	/// The queued animations.
	/// </summary>
	private List<AnimationDestination> queuedAnimations;

	/// <summary>
	/// The animation that is currently being stepped through.
	/// Null if inactive.
	/// </summary>
	private AnimationDestination currentAnimation {
		get { 
			if (queuedAnimations.Count == 0) {
				return null;
			}
			else {
				return queuedAnimations [0];
			}
		}
	}

	/// <summary>
	/// Remaining duration on the current animation.
	/// </summary>
	private Timer currentTimer;

	public AnimationQueue (Transform animating) {
		queuedAnimations = new List<AnimationDestination> ();
		objectTransform = animating;
	}

	/// <summary>
	/// Add an animation to the queue.
	/// </summary>
	public void QueueAnimation (AnimationDestination destination) {
		if (destination != null) {
			queuedAnimations.Add (destination);
			// If the thing that was just added was the first animation
			if (queuedAnimations.Count == 1) {
				BeginAnimation ();
			}
		}
	}

	/// <summary>
	/// If there is nothing currently animating, slot in the first thing from the queue.
	/// </summary>
	private void BeginAnimation () {
		initialValues = AnimationDestination.CreateOrigin (objectTransform);
		currentTimer = new Timer (currentAnimation.duration);
	}

	/// <summary>
	/// Called once per frame by the AnimationManager.
	/// </summary>
	public void AnimationUpdate () {
		currentTimer.Tick ();
		if (currentAnimation.position != null) {
			objectTransform.position = Interpolation.Interpolate (initialValues.position.vector, currentAnimation.position.vector, currentTimer.ratio, currentAnimation.interpolationMethod);
		}
		if (currentAnimation.localScale != null) {
			objectTransform.localScale = Interpolation.Interpolate (initialValues.localScale.vector, currentAnimation.localScale.vector, currentTimer.ratio, currentAnimation.interpolationMethod);
		}
		if (currentAnimation.rotation != null) {
			objectTransform.rotation = Interpolation.Interpolate (initialValues.rotation.quaternion, currentAnimation.rotation.quaternion, currentTimer.ratio, currentAnimation.interpolationMethod);
		}

		if (!currentTimer.active) {
			EndCurrentAnimation ();
		}
	}

	/// <summary>
	/// Advance the current animation to the end. Useful for cleaning up messy animation ends.
	/// </summary>
	public void EndCurrentAnimation () {
		currentTimer.Disable ();

		//snap
		if (currentAnimation.position != null) {
			objectTransform.position = currentAnimation.position.vector;
		}
		if (currentAnimation.localScale != null) {
			objectTransform.localScale = currentAnimation.localScale.vector;
		}
		if (currentAnimation.rotation != null) {
			objectTransform.rotation = currentAnimation.rotation.quaternion;
		}

		queuedAnimations.RemoveAt (0);
		if (active) {
			BeginAnimation ();
		}
	}

	/// <summary>
	/// Are there unfinished animations in the queue?
	/// </summary>
	public bool active {
		get {
			return currentAnimation != null;
		}
	}
}
