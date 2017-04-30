using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A standoff between a cat and a dog, and the probability of that cat being detected.
/// </summary>
public class DetectionMatchup {
	public Cat catInDanger;
	public Dog watchingDog;
	public float danger;

	public DetectionMatchup (Cat cat, Dog dog, float danger) {
		catInDanger = cat;
		watchingDog = dog;
		this.danger = danger;
	}

	/// <summary>
	/// Points the camera to the halfway point between the two.
	/// </summary>
	public void CameraHalfway () {
		CameraOverheadControl.SetCamFocusPoint (catInDanger.myTile.topCenterPoint.HalfwayTo (watchingDog.myTile.topCenterPoint));
	}

	/// <summary>
	/// Returns true if the cat should be busted.
	/// </summary>
	public bool SimulateDetectionCheck (out float rolledChance) {
		rolledChance = Random.value;
		return rolledChance < danger;
	}
	/// <summary>
	/// Returns true if the cat should be busted.
	/// </summary>
	public bool SimulateDetectionCheck () {
		return Random.value < danger;
	}
}
