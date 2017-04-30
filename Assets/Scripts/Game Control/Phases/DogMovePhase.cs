using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dog equivalent to CatExecutePhase.
/// Exits: DogTurnDetectionPhase
/// </summary>
public class DogMovePhase : GameControlPhase {
	/// <summary>
	/// Used for static TakeControl
	/// </summary>
	private static DogMovePhase staticInstance;
	/// <summary>
	/// Puts the DogMovePhase in control.
	/// </summary>
	public static void TakeControl (Dog selectedDog, List<Tile> tilePath) {
		staticInstance.selectedDog = selectedDog;
		staticInstance.tilePath = tilePath;
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}

	/// <summary>
	/// The dog that will move.
	/// </summary>
	private Dog selectedDog;

	/// <summary>
	/// The ordered path the dog will take.
	/// </summary>
	private List<Tile> tilePath;

	//[SerializeField] private AudioSource purrSound;

	override public void OnTakeControl () {
		CameraOverheadControl.SetCamFollowTarget (selectedDog.transform);
		//purrSound.Play ();
		//selectedCat.walkingAnimation = true;
	}

	override public void StandardUpdate () {
		if (Input.GetMouseButtonDown (0)) {
			Time.timeScale = 100f;
		}
	}

	override public void ControlUpdate () {
		if (tilePath.Count > 0) {
			selectedDog.MoveTo (tilePath [0]);
			tilePath.RemoveAt (0);
		}
		else {
			DogTurnDetectionPhase.TakeControl ();
		}
	}

	override public void OnLeaveControl () {
		Time.timeScale = 1f;
		UIManager.masterInfoBox.headerText = "";
		UIManager.masterInfoBox.ClearAllData ();
		CameraOverheadControl.StopFollowing ();
		//purrSound.Stop ();
		selectedDog.grayedOut = true;
		//selectedCat.walkingAnimation = false;
	}
}
