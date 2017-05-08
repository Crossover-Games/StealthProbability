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
		buttonThisTurn = null;
		triggerPressurePlatesThisTurn = null;
		immediateFail = false;
		staticInstance.selectedCat = selectedCat;
		staticInstance.tilePath = tilePath;
		staticInstance.InstanceTakeControl ();
	}

	private static bool? triggerPressurePlatesThisTurn;
	private static ButtonTileOnePress buttonThisTurn;
	private static bool immediateFail;

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
		if (tilePath.Count > 0) {
			purrSound.Play ();
			selectedCat.walkingAnimation = true;
		}
		selectedCat.stealthStacks = selectedCat.maxEnergy - tilePath.Count;
	}

	override public void StandardUpdate () {
		if (Input.GetKey (KeyCode.Space) && triggerPressurePlatesThisTurn == null) {
			Time.timeScale = 100f;
		}
	}

	override public void ControlUpdate () {
		if (tilePath.Count > 0) {
			if (tilePath [0] is ButtonTileOnePress) {
				buttonThisTurn = tilePath [0] as ButtonTileOnePress;
			}
			if (tilePath [0].occupant != null && tilePath [0].occupant is Dog) {
				immediateFail = true;
				tilePath [0].occupant.PlaySound ();
			}
			selectedCat.MoveTo (tilePath [0]);
			tilePath.RemoveAt (0);
		}
		else if (triggerPressurePlatesThisTurn == null) {   // always comes after movement
			while (charactersCrossed.Count > 0) {
				charactersCrossed.Pop ().FindMyTile ();
			}
			Time.timeScale = 1f;
			UIManager.masterInfoBox.headerText = "";
			UIManager.masterInfoBox.ClearAllData ();
			CameraOverheadControl.StopFollowing ();
			purrSound.Stop ();
			if (GameBrain.dogManager.allCharacters.Length == 0 && GameBrain.catManager.availableCharacters.Length == 1) {
				selectedCat.SetPseudoGray (true);
			}
			else {
				selectedCat.grayedOut = true;
			}

			selectedCat.walkingAnimation = false;

			if (!PressurePlateMaster.actionAlreadyExecuted && PressurePlateMaster.AllButtonsActivated ()) {
				PressurePlateMaster.CameraToFocusPoint ();
				AnimationManager.AddStallTime (staticInstance.transform, .5f);
				triggerPressurePlatesThisTurn = true;
			}
			else {
				if (buttonThisTurn != null) {
					buttonThisTurn.ActivateVisuals ();
					buttonThisTurn.CameraToFocusPoint ();
					AnimationManager.AddStallTime (staticInstance.transform, .5f);
				}
				triggerPressurePlatesThisTurn = false;
			}
		}
		else if (buttonThisTurn != null) {
			buttonThisTurn.ActivateAll ();
			AnimationManager.AddStallTime (staticInstance.transform, 1.5f);
			buttonThisTurn = null;
		}
		else if (triggerPressurePlatesThisTurn.GetValueOrDefault ()) {  //trigger sprinklers
			PressurePlateMaster.ActivateAll ();
			AnimationManager.AddStallTime (staticInstance.transform, 1.5f);
			triggerPressurePlatesThisTurn = false;
		}
		else {
			CatTurnDetectionPhase.TakeControl (selectedCat, immediateFail);
		}
	}

	override public void OnLeaveControl () {
		Time.timeScale = 1f;
	}
}
