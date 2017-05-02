using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The bar that visualizes the detection chance.
/// </summary>
public class DetectionMeter : MonoBehaviour {

	private static DetectionMeter staticInstance;

	private static float fadeTime { get { return 0.25f; } }
	private static float waitTime { get { return 0.5f; } }
	private static float rollCycleRate { get { return 0.2f; } }

	[SerializeField] private Transform dangerBar;
	[SerializeField] private Transform safeBar;
	[SerializeField] private Transform pointerTransform;


	[SerializeField] private Renderer dangerBarRenderer;
	[SerializeField] private SpriteRenderer holoSpriteRenderer;
	[SerializeField] private Renderer pointerRenderer;

	[SerializeField] private TextMesh percentText;
	[SerializeField] private TextMesh percentTextShadow;
	[SerializeField] private TextMesh wildCardText;
	[SerializeField] private TextMesh wildCardTextShadow;

	private static string percentHack {
		get { return "<size=80> o/o</size>"; }
	}
	private static string wildCardReady {
		get { return "<i>WILD CARD READY</i>"; }
	}
	private static string wildCardDepleted {
		get { return "Wild card depleted"; }
	}
	private static string deployingWildCard {
		get { return "<i>Deploying wild card...</i>"; }
	}
	private static string rektText {
		get { return "<i>EMERGENCY EXIT!!!</i>"; }
	}


	void Awake () {
		staticInstance = this;
	}

	private static Vector3 LandHere (float rolledChance) {
		return Vector3.right * (1f - rolledChance);
	}

	/// <summary>
	/// Animates the visualizer.
	/// </summary>
	public static void AnimateRoll (float danger, float rolledChance, bool failed, Cat cat) {
		Color dangerColor = TileDangerData.DangerToColor (danger);
		new ChangeRendererEmissionColor (staticInstance.pointerRenderer, Color.black).Execute ();

		string pctxt = Mathf.FloorToInt (danger * 100).ToString () + percentHack; ;
		staticInstance.percentText.text = pctxt;
		staticInstance.percentTextShadow.text = pctxt;
		staticInstance.percentText.color = dangerColor;

		if (cat.hasWildCard) {
			staticInstance.wildCardText.color = Color.white;
			staticInstance.wildCardText.text = wildCardReady;
			staticInstance.wildCardTextShadow.text = wildCardReady;
		}
		else {
			staticInstance.wildCardText.color = Color.gray;
			staticInstance.wildCardText.text = wildCardDepleted;
			staticInstance.wildCardTextShadow.text = wildCardDepleted;
		}

		staticInstance.dangerBarRenderer.material.color = dangerColor.AlphaDifferent (0.5f);
		staticInstance.holoSpriteRenderer.color = dangerColor.AlphaDifferent (0.5f);
		staticInstance.pointerTransform.localPosition = Vector3.zero;
		staticInstance.dangerBar.localScale = new Vector3 (-danger, 1f, 1f);
		staticInstance.safeBar.localScale = new Vector3 (1f - danger, 1f, 1f);

		// fade in
		AnimationManager.AddAnimation (staticInstance.transform, new AnimationDestination (null, null, Vector3.one, fadeTime, InterpolationMethod.Quadratic), false);
		AnimationManager.AddStallTime (staticInstance.pointerTransform, fadeTime, false);

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

		IActionCommand allFailCommands = null;
		if (failed) {
			Stack<IActionCommand> failCommands = new Stack<IActionCommand> ();
			failCommands.Push (new ChangeRendererEmissionColor (staticInstance.pointerRenderer, dangerColor));
			if (cat.hasWildCard) {
				failCommands.Push (new ChangeTextMeshCommand (staticInstance.wildCardText, deployingWildCard, Color.cyan));
				failCommands.Push (new ChangeTextMeshCommand (staticInstance.wildCardTextShadow, deployingWildCard));
			}
			else {
				failCommands.Push (new ChangeTextMeshCommand (staticInstance.wildCardText, rektText, Color.red));
				failCommands.Push (new ChangeTextMeshCommand (staticInstance.wildCardTextShadow, rektText));
			}
			allFailCommands = new ExecuteMultipleCommands (failCommands);
		}

		// Land on probability
		AnimationManager.AddAnimation (staticInstance.pointerTransform, new AnimationDestination (LandHere (rolledChance), null, null, finalLandTimeCoeff * rollCycleRate, InterpolationMethod.Linear, true, allFailCommands), true);
		AnimationManager.AddStallTime (staticInstance.transform, finalLandTimeCoeff * rollCycleRate, true);

		if (failed) {
			// take it in.
			AnimationManager.AddStallTime (staticInstance.pointerTransform, rollCycleRate, true);
			AnimationManager.AddStallTime (staticInstance.transform, rollCycleRate, true);

			// bounce
			AnimationManager.AddAnimation (staticInstance.pointerTransform, new AnimationDestination (LandHere (rolledChance) + Vector3.up * 0.1f, null, null, rollCycleRate * 0.5f, InterpolationMethod.Quadratic, true), true);
			AnimationManager.AddAnimation (staticInstance.pointerTransform, new AnimationDestination (LandHere (rolledChance), null, null, rollCycleRate * 0.5f, InterpolationMethod.Quadratic, true), true);
			AnimationManager.AddAnimation (staticInstance.pointerTransform, new AnimationDestination (LandHere (rolledChance) + Vector3.up * 0.1f, null, null, rollCycleRate * 0.5f, InterpolationMethod.Quadratic, true), true);
			AnimationManager.AddAnimation (staticInstance.pointerTransform, new AnimationDestination (LandHere (rolledChance), null, null, rollCycleRate * 0.5f, InterpolationMethod.Quadratic, true), true);
			AnimationManager.AddStallTime (staticInstance.transform, rollCycleRate * 2f, true);
		}

		// look at prob
		AnimationManager.AddStallTime (staticInstance.pointerTransform, waitTime, true);
		AnimationManager.AddStallTime (staticInstance.transform, waitTime, true, new PointCameraCommand (cat.myTile.topCenterPoint));

		// Fade out
		AnimationManager.AddAnimation (staticInstance.transform, new AnimationDestination (null, null, Vector3.zero, fadeTime, InterpolationMethod.SquareRoot), true);
	}
}
