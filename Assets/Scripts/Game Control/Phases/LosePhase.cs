using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The phase indicating that you lost the game.
/// Exits: None!
/// </summary>
public class LosePhase : GameControlPhase {
	/// <summary>
	/// Used for static TakeControl
	/// </summary>
	private static LosePhase staticInstance;
	/// <summary>
	/// Puts the LosePhase in control
	/// </summary>
	public static void TakeControl () {
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}

	[SerializeField] private GameObject loseEffect;
	[SerializeField] private RestartButton restartButton;

	public override void OnTakeControl () {
		loseEffect.SetActive (true);
		restartButton.BeginFadeIn ();
		LoadLoadingScreen.PrimeRestartLoadingScreen ();
	}
}
