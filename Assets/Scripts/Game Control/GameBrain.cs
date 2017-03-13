using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the flow of what is allowed to happen in the game. Allows one GameControlPhase to operate at a time, and allows switching between them.
/// </summary>
public class GameBrain : MonoBehaviour {

	// only serialized so that you can choose the starting phase.
	[SerializeField] private GameControlPhase inControl = null;

	[SerializeField] private UniversalTileManager universalTileManager;
	/// <summary>
	/// Reference to the scene's universal tile manager.
	/// </summary>
	public UniversalTileManager tileManager {
		get { return universalTileManager; }
	}
		
	[SerializeField] private AnimationManager myAnimationManager;
	/// <summary>
	/// The unit that animates movement for pieces on the board.
	/// </summary>
	public AnimationManager animationManager {
		get { return myAnimationManager; }
	}

	[SerializeField] private CameraOverheadControl myCameraControl;
	/// <summary>
	/// Contains methods to control the camera.
	/// </summary>
	public CameraOverheadControl cameraControl {
		get{ return myCameraControl; }
	}

	/// <summary>
	/// Helper method for GameControlPhase.TakeControl(). Kicks the previous phase out of control, calls its OnLeaveControl, then puts the new phase in charge and calls its OnTakeControl.
	/// </summary>
	public void AssignControl (GameControlPhase phase) {
		if (inControl != null) {
			inControl.OnLeaveControl ();
		}
		phase.OnTakeControl ();
		inControl = phase;
	}

	void Update () {
		if (myAnimationManager.activelyAnimating) {
			myAnimationManager.AnimationUpdate ();
		}
		else if (inControl != null) {
			inControl.ControlUpdate ();
		}
	}

	/// <summary>
	/// Calls the operating GameControlPhase's TileClickEvent().
	/// </summary>
	public void NotifyBrainTileClickEvent (Tile t) {
		if (inControl != null) {
			inControl.TileClickEvent (t);
		}
	}

	/// <summary>
	/// Calls the operating GameControlPhase's TileDoubleClickEvent().
	/// </summary>
	public void NotifyBrainTileDoubleClickEvent (Tile t) {
		if (inControl != null) {
			inControl.TileDoubleClickEvent (t);
		}
	}

	/// <summary>
	/// Calls the operating GameControlPhase's MouseOverChangeEvent().
	/// </summary>
	public void NotifyBrainMouseOverChangeEvent () {
		if (inControl != null) {
			inControl.MouseOverChangeEvent ();
		}
	}
}
