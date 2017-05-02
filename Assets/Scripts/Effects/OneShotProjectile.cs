using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotProjectile : MonoBehaviour {

	private static OneShotProjectile staticInstance;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake () {
		staticInstance = this;
	}

	public static void LaunchAtPosition (Vector3 position) {
		staticInstance.transform.position = position;
		staticInstance.LaunchSelf ();
		AnimationManager.AddStallTime (staticInstance.transform, 1f, false);
	}

	[ContextMenu ("Launch at current position")]
	private void LaunchSelf () {
		if (gameObject.activeSelf) {
			gameObject.SetActive (false);
		}
		gameObject.SetActive (true);
	}
}
