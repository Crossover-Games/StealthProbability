using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The bar that visualizes the detection chance.
/// </summary>
public class DetectionMeter : MonoBehaviour {

	private static DetectionMeter staticInstance;

	private static float fadeTime { get { return 0.33f; } }
	private static float rollCycleRate { get { return 0.2f; } }

	[SerializeField] private Transform dangerBar;
	[SerializeField] private Transform safeBar;
	[SerializeField] private Transform pointerTransform;


	[SerializeField] private Renderer dangerBarRenderer;
	[SerializeField] private SpriteRenderer holoSpriteRenderer;


	void Awake () {
		staticInstance = this;
	}

	private static Vector3 LandHere (float rolledChance) {
		return Vector3.right * (1f - rolledChance);
	}

	/// <summary>
	/// Animates the visualizer.
	/// </summary>
	public static void AnimateRoll (float danger, float rolledChance, bool failed) {
		Color dangerColor = TileDangerData.DangerToColor (danger);
		staticInstance.dangerBarRenderer.material.color = dangerColor;
		staticInstance.holoSpriteRenderer.color = dangerColor;
		staticInstance.pointerTransform.localPosition = Vector3.zero;
		staticInstance.dangerBar.localScale = new Vector3 (-danger, 1f, 1f);
		staticInstance.safeBar.localScale = new Vector3 (1f - danger, 1f, 1f);

		// fade in
		AnimationManager.AddAnimation (staticInstance.transform, new AnimationDestination (null, null, Vector3.one, fadeTime, InterpolationMethod.SquareRoot), false);
		AnimationManager.AddStallTime (staticInstance.transform, fadeTime * 0.5f, true);

		AnimationManager.AddStallTime (staticInstance.pointerTransform, fadeTime * 1.5f, false);

		// back and forth
		bool cycle = true;
		int iterations = UnityEngine.Random.Range (2, 6);
		for (int x = 0; x < iterations; x++) {
			Vector3 destination;
			if (cycle) {
				destination = Vector3.right;
			}
			else {
				destination = Vector3.zero;
			}
			AnimationManager.AddAnimation (staticInstance.pointerTransform, new AnimationDestination (destination, null, null, rollCycleRate, InterpolationMethod.Linear, true), true);
			cycle = !cycle;
		}
		AnimationManager.AddStallTime (staticInstance.transform, iterations * rollCycleRate, true);

		float finalLandTimeCoeff;
		if (cycle) {
			finalLandTimeCoeff = 1f - rolledChance;
		}
		else {
			finalLandTimeCoeff = rolledChance;
		}
		// Land on probability
		AnimationManager.AddAnimation (staticInstance.pointerTransform, new AnimationDestination (LandHere (rolledChance), null, null, finalLandTimeCoeff * rollCycleRate, InterpolationMethod.Quadratic, true), true);
		AnimationManager.AddStallTime (staticInstance.transform, finalLandTimeCoeff * rollCycleRate, true);

		if (failed) {
			AnimationManager.AddAnimation (staticInstance.pointerTransform, new AnimationDestination (LandHere (rolledChance) + Vector3.up * 0.1f, null, null, rollCycleRate * 0.5f, InterpolationMethod.Quadratic, true), true);
			AnimationManager.AddAnimation (staticInstance.pointerTransform, new AnimationDestination (LandHere (rolledChance), null, null, rollCycleRate * 0.5f, InterpolationMethod.Quadratic, true), true);
			AnimationManager.AddAnimation (staticInstance.pointerTransform, new AnimationDestination (LandHere (rolledChance) + Vector3.up * 0.1f, null, null, rollCycleRate * 0.5f, InterpolationMethod.Quadratic, true), true);
			AnimationManager.AddAnimation (staticInstance.pointerTransform, new AnimationDestination (LandHere (rolledChance), null, null, rollCycleRate * 0.5f, InterpolationMethod.Quadratic, true), true);
			AnimationManager.AddStallTime (staticInstance.transform, rollCycleRate * 2f, true);
		}

		// look at prob
		AnimationManager.AddStallTime (staticInstance.pointerTransform, rollCycleRate, true);
		AnimationManager.AddStallTime (staticInstance.transform, rollCycleRate, true);

		// Fade out
		AnimationManager.AddAnimation (staticInstance.transform, new AnimationDestination (null, null, Vector3.zero, fadeTime, InterpolationMethod.SquareRoot), true);
	}
}
