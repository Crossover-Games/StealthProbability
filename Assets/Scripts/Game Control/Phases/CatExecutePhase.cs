using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In this phase, the cat just carries out the orders it was given.
/// Exits: PlayerTurnIdlePhase
/// </summary>
public class CatExecutePhase : GameControlPhase {
	/// <summary>
	/// Used for static TakeControl
	/// </summary>
	private static CatExecutePhase staticInstance;
	/// <summary>
	/// Puts the CatExecutePhase in control.
	/// </summary>
	public static void TakeControl (Cat selectedCat, List<Tile> tilePath) {
		staticInstance.selectedCat = selectedCat;
		staticInstance.tilePath = tilePath;
		staticInstance.InstanceTakeControl ();
	}
	void Awake () {
		staticInstance = this;
	}
	/// <summary>
	/// Target cat.
	/// </summary>
	private Cat selectedCat;

	/// <summary>
	/// The ordered path the cat will take.
	/// </summary>
	private List<Tile> tilePath;

	/// <summary>
	/// The cats that were stepped over and will have to have their tiles re-registered.
	/// </summary>
	public static Stack<GameCharacter> charactersCrossed = new Stack<GameCharacter> ();

	[SerializeField] private AudioSource purrSound;

	override public void OnTakeControl () {
		CameraOverheadControl.SetCamFollowTarget (selectedCat.transform);
		purrSound.Play ();
		selectedCat.walkingAnimation = true;
	}

	override public void StandardUpdate () {
		if (Input.GetKey (KeyCode.Space)) {
			Time.timeScale = 100f;
		}
	}

	override public void ControlUpdate () {
		if (tilePath.Count > 0) {
			selectedCat.MoveTo (tilePath [0]);
			tilePath.RemoveAt (0);
		}
		else {
			while (charactersCrossed.Count > 0) {
				charactersCrossed.Pop ().FindMyTile ();
			}
			CatTurnDetectionPhase.TakeControl ();
		}
	}

	override public void OnLeaveControl () {
		Time.timeScale = 1f;
		UIManager.masterInfoBox.headerText = "";
		UIManager.masterInfoBox.ClearAllData ();
		CameraOverheadControl.StopFollowing ();
		purrSound.Stop ();
    selectedCat.decrementWetTurns();
		if (GameBrain.dogManager.allCharacters.Length == 0 && GameBrain.catManager.availableCharacters.Length == 1) {
			selectedCat.SetPseudoGray (true);
		}
		else {
			selectedCat.grayedOut = true;
		}

		selectedCat.walkingAnimation = false;
	}
}
