using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The phase indicating that you won the game.
/// Exits: None!
/// </summary>
public class VictoryPhase : GameControlPhase {
	/// <summary>
	/// Used for static TakeControl
	/// </summary>
	private static VictoryPhase staticInstance;
	/// <summary>
	/// Puts the VictoryPhase in control
	/// </summary>
	public static void TakeControl () {
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}

	[SerializeField] private GameObject winEffect;
	[SerializeField] private NextLevelButton winButton;

	public override void OnTakeControl () {
		LoadLoadingScreen.PrimeNextLoadingScreen ();
		winEffect.SetActive (true);
		winButton.BeginFadeIn ();
	}
}
