using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The bar that visualizes the detection chance.
/// </summary>
public class DetectionMeter : MonoBehaviour {

	private static DetectionMeter staticInstance;

	private static float fadeTime { get { return 0.25f; } }
	private static float adjustTime { get { return 1f; } }
	private static float waitTime { get { return 0.5f; } }
	private static float negligibleTime { get { return 0.001f; } }
	private static float rollCycleRate { get { return 0.2f; } }

	[SerializeField] private Transform dangerBar;
	[SerializeField] private Transform safeBar;
	[SerializeField] private Transform pointerTransform;


	[SerializeField] private Renderer dangerBarRenderer;
	[SerializeField] private Renderer pointerRenderer;

	[SerializeField] private TextMesh percentText;
	[SerializeField] private TextMesh percentTextShadow;
	[SerializeField] private TextMesh wildCardText;
	[SerializeField] private TextMesh wildCardTextShadow;

	private static string percentHack {
		get { return "<size=80> o/o</size>"; }
	}
	private static string wildCardReady {
		get { return "<i>SECOND CHANCE READY</i>"; }
	}
	private static string wildCardDepleted {
		get { return "Second chance depleted"; }
	}
	private static string deployingWildCard {
		get { return "<i>Deploying second chance...</i>"; }
	}
	private static string rektText {
		get { return "<i>EMERGENCY EXIT!!!</i>"; }
	}
	private static string waterText {
		get { return "Negating probability due to wetness"; }
	}
	private static string reductionText {
		get { return "Stealth bonus for extra energy!"; }
	}


	void Awake () {
		staticInstance = this;
	}

	private static Vector3 LandHere (float rolledChance) {
		return Vector3.right * (1f - rolledChance);
	}

	/// <summary>
	/// Animates the visualizer. True if failed.
	/// </summary>
	public static bool ConductRollAndAnimate (DetectionMatchup matchup) {
		return staticInstance.RollHelper (matchup);
	}

	private void QueueAdjustBars (float updatedDanger) {
		AnimationManager.AddAnimation (dangerBar, new AnimationDestination (null, null, new Vector3 (-updatedDanger, 1f, 1f), adjustTime, InterpolationMethod.Quadratic), true);
		AnimationManager.AddAnimation (safeBar, new AnimationDestination (null, null, new Vector3 (1f - updatedDanger, 1f, 1f), adjustTime, InterpolationMethod.Quadratic), true);
		AnimationManager.AddStallTime (transform, adjustTime, true);
		AnimationManager.AddStallTime (pointerTransform, adjustTime, true);
	}

	private ExecuteMultipleCommands ChangeMainText (string text, Color? color = null) {
		Stack<IActionCommand> textCommands = new Stack<IActionCommand> ();
		textCommands.Push (new ChangeTextMeshCommand (wildCardText, text, color));
		textCommands.Push (new ChangeTextMeshCommand (wildCardTextShadow, text));
		return new ExecuteMultipleCommands (textCommands);
	}

	private ExecuteMultipleCommands ChangeTextForWildCard (DetectionMatchup matchup) {
		if (matchup.catInDanger.hasWildCard) {
			return ChangeMainText (wildCardReady, Color.white);
		}
		else {
			return ChangeMainText (wildCardDepleted, Color.gray);
		}
	}

	private void QueueAction (IActionCommand command) {
		AnimationManager.AddStallTime (transform, negligibleTime, true);
		AnimationManager.AddStallTime (pointerTransform, negligibleTime, true);
		AnimationManager.AddStallTime (dangerBar, negligibleTime, true);
		AnimationManager.AddStallTime (safeBar, negligibleTime, true, command);
	}

	private void StallAll (float duration) {
		AnimationManager.AddStallTime (pointerTransform, duration, true);
		AnimationManager.AddStallTime (transform, duration, true);
		AnimationManager.AddStallTime (dangerBar, duration, true);
		AnimationManager.AddStallTime (safeBar, duration, true);
	}

	private bool RollHelper (DetectionMatchup matchup) {
		Color dangerColor = TileDangerData.DangerToColor (matchup.danger);
		new ChangeRendererEmissionColor (pointerRenderer, Color.black).Execute ();

		string percentRichText = Mathf.FloorToInt (matchup.danger * 100).ToString () + percentHack; ;
		percentText.text = percentRichText;
		percentTextShadow.text = percentRichText;
		percentText.color = dangerColor;

		ChangeTextForWildCard (matchup).Execute ();

		float effectiveDanger = matchup.danger;
		float rolledChance = Random.value;

		dangerBarRenderer.material.color = dangerColor.AlphaDifferent (0.5f);
		pointerTransform.localPosition = Vector3.zero;
		dangerBar.localScale = new Vector3 (-effectiveDanger, 1f, 1f);
		safeBar.localScale = new Vector3 (1f - effectiveDanger, 1f, 1f);

		// fade in
		AnimationManager.AddAnimation (transform, new AnimationDestination (null, null, Vector3.one, fadeTime, InterpolationMethod.Quadratic), false);
		AnimationManager.AddStallTime (pointerTransform, fadeTime, false);
		AnimationManager.AddStallTime (dangerBar, negligibleTime, false);
		AnimationManager.AddStallTime (safeBar, negligibleTime, false);

		// scale for inversion
		if (matchup.catInDanger.isWet) {
			StallAll (waitTime);
			QueueAction (ChangeMainText (waterText, WetFloor.waterColor));
			effectiveDanger = 1f - effectiveDanger;
			QueueAdjustBars (effectiveDanger);
			QueueAction (ChangeTextForWildCard (matchup));
		}

		// scale for danger reduction
		int stealthStacksOnCat = 0;
		float stealthMultiplier = 0.1f;
		if (stealthStacksOnCat > 0) {
			QueueAction (ChangeMainText (reductionText, Color.green));
			effectiveDanger = Mathf.Clamp01 (effectiveDanger - stealthStacksOnCat * stealthMultiplier);
			QueueAdjustBars (effectiveDanger);
			QueueAction (ChangeTextForWildCard (matchup));
		}

		bool failed = rolledChance < effectiveDanger;

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
			AnimationManager.AddAnimation (pointerTransform, new AnimationDestination (destination, null, null, rollCycleRate, InterpolationMethod.Linear, true), true);
			cycle = !cycle;
		}
		AnimationManager.AddStallTime (transform, iterations * rollCycleRate, true);

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
			failCommands.Push (new ChangeRendererEmissionColor (pointerRenderer, dangerColor));
			if (matchup.catInDanger.hasWildCard) {
				failCommands.Push (new ChangeTextMeshCommand (wildCardText, deployingWildCard, Color.cyan));
				failCommands.Push (new ChangeTextMeshCommand (wildCardTextShadow, deployingWildCard));
			}
			else {
				failCommands.Push (new ChangeTextMeshCommand (wildCardText, rektText, Color.red));
				failCommands.Push (new ChangeTextMeshCommand (wildCardTextShadow, rektText));
			}
			allFailCommands = new ExecuteMultipleCommands (failCommands);
		}

		// Land on probability
		AnimationManager.AddAnimation (pointerTransform, new AnimationDestination (LandHere (rolledChance), null, null, finalLandTimeCoeff * rollCycleRate, InterpolationMethod.Linear, true, allFailCommands), true);
		AnimationManager.AddStallTime (transform, finalLandTimeCoeff * rollCycleRate, true);

		if (failed) {
			// take it in.
			StallAll (rollCycleRate);

			// bounce
			AnimationManager.AddAnimation (pointerTransform, new AnimationDestination (LandHere (rolledChance) + Vector3.up * 0.1f, null, null, rollCycleRate * 0.5f, InterpolationMethod.Quadratic, true), true);
			AnimationManager.AddAnimation (pointerTransform, new AnimationDestination (LandHere (rolledChance), null, null, rollCycleRate * 0.5f, InterpolationMethod.Quadratic, true), true);
			AnimationManager.AddAnimation (pointerTransform, new AnimationDestination (LandHere (rolledChance) + Vector3.up * 0.1f, null, null, rollCycleRate * 0.5f, InterpolationMethod.Quadratic, true), true);
			AnimationManager.AddAnimation (pointerTransform, new AnimationDestination (LandHere (rolledChance), null, null, rollCycleRate * 0.5f, InterpolationMethod.Quadratic, true), true);
			AnimationManager.AddStallTime (transform, rollCycleRate * 2f, true);
		}

		// look at prob
		StallAll (waitTime);

		// Fade out
		AnimationManager.AddAnimation (transform, new AnimationDestination (null, null, Vector3.zero, fadeTime, InterpolationMethod.SquareRoot), true);

		return failed;
	}
}
