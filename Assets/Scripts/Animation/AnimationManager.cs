using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// One per scene. Manages all animating objects.
/// </summary>
public class AnimationManager : MonoBehaviour {
	private static List<AnimationQueue> allObjects = new List<AnimationQueue> ();

	/// <summary>
	/// Are there any objects currently animating?
	/// </summary>
	public static bool active {
		get { return allObjects.Count > 0; }
	}

	/// <summary>
	/// MonoBehaviour override.
	/// Steps through all animating objects, removes completed ones.
	/// </summary>
	void Update () {
		if (allObjects.Count > 0) {
			AnimationQueue [] currentObjects = allObjects.ToArray ();
			foreach (AnimationQueue anim in currentObjects) {
				anim.AnimationUpdate ();
				if (!anim.active) {
					allObjects.Remove (anim);
				}
			}
		}
	}

	/// <summary>
	/// Registers an object to animate. If queue is true, it will queue an animation on that object. Otherwise, it will stop that object and overwrite its animation queue.
	/// </summary>
	public static void AddAnimation (Transform animatingObject, AnimationDestination destination, bool queue = true) {
		allObjects.Exists ((AnimationQueue obj) => obj.animatingObject == animatingObject);

		if (!queue) {
			allObjects.RemoveAll ((AnimationQueue obj) => obj.animatingObject == animatingObject);
			AnimationQueue newQueue = new AnimationQueue (animatingObject);
			newQueue.QueueAnimation (destination);
			allObjects.Add (newQueue);
		}
		else {
			int existingIndex = allObjects.FindIndex ((AnimationQueue obj) => obj.animatingObject == animatingObject);
			if (existingIndex != -1) {
				allObjects [existingIndex].QueueAnimation (destination);
			}
			else {
				AnimationQueue newQueue = new AnimationQueue (animatingObject);
				newQueue.QueueAnimation (destination);
				allObjects.Add (newQueue);
			}
		}
	}

	/// <summary>
	/// Stall an animation for a set amount of time.
	/// </summary>
	public static void AddStallTime (Transform t, float time, bool queue = true) {
		AddAnimation (t, new AnimationDestination (null, null, null, time, InterpolationMethod.Linear), queue);
	}

	/// <summary>
	/// Terrible. Adds delay time without animating anything.
	/// </summary>
	public static void DummyTime (float time) {
		AddAnimation (new GameObject ("DUMMY").transform, new AnimationDestination (null, null, null, time, InterpolationMethod.Linear));
	}
}
