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
		staticInstance.goteem = false;
		staticInstance.trampled = new Stack<Cat> ();
		staticInstance.selectedDog = selectedDog;
		staticInstance.tilePath = tilePath;
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}

	private bool goteem;
	private Stack<Cat> trampled;

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
		if (Input.GetKey (KeyCode.Space)) {
			Time.timeScale = 100f;
		}
	}

	override public void ControlUpdate () {
		if (trampled.Count > 0) {
			goteem = true;
			Cat selectedCat = trampled.Pop ();
			AnimationManager.AddAnimation (selectedCat.transform, new AnimationDestination (null, null, Vector3.zero, 1f, InterpolationMethod.SquareRoot));
			GameBrain.catManager.Remove (selectedCat);
		}
		else if (tilePath.Count > 0) {
			if (tilePath [0].occupant != null && tilePath [0].occupant is Cat) {
				trampled.Push (tilePath [0].occupant as Cat);
			}
			selectedDog.MoveTo (tilePath [0]);
			tilePath.RemoveAt (0);
		}
		else {
			if (goteem) {
				LosePhase.TakeControl ();
			}
			else {
				DogTurnDetectionPhase.TakeControl ();
			}
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
