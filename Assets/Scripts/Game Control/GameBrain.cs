﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the flow of what is allowed to happen in the game. Allows one GameControlPhase to operate at a time, and allows switching between them.
/// </summary>
public class GameBrain : MonoBehaviour {

	/// <summary>
	/// forcibly stops the controlling object and forces the camera on the first cat
	/// </summary>
	public void JAMHACK () {
		inControl = null;
		myCameraControl.SetCamFocusPoint (m_catManager.allCharacters [0].myTile.topCenterPoint);
	}


	[Tooltip ("The first phase. Its OnTakeControl will be called.")]
	[SerializeField] private GameControlPhase startingPhase;

	/// <summary>
	/// The phase that is currently controlling the game.
	/// </summary>
	private static GameControlPhase inControl = null;


	// ---REFERENCES

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

	private TeamMananger<Cat> m_catManager;
	/// <summary>
	/// Has knowledge of all cats.
	/// </summary>
	public TeamMananger<Cat> catManager {
		get { return m_catManager; }
	}

	private TeamMananger<Dog> m_dogManager;
	/// <summary>
	/// Has knowledge of all dogs.
	/// </summary>
	public TeamMananger<Dog> dogManager {
		get { return m_dogManager; }
	}

	[SerializeField] private UIManager m_uiManager;
	/// <summary>
	/// Gets the user interface manager.
	/// </summary>
	public UIManager uiManager {
		get { return m_uiManager; }
	}

	// ---MONOBEHAVIOUR OVERRIDES

	[Tooltip ("Parent of all cats in the scene.")]
	[SerializeField] private GameObject catParent;

	[Tooltip ("Parent of all dogs in the scene.")]
	[SerializeField] private GameObject dogParent;

	void Awake () {
		m_catManager = new TeamMananger<Cat> (new List<Cat> (catParent.GetComponentsInChildren<Cat> ()));
		m_dogManager = new TeamMananger<Dog> (new List<Dog> (dogParent.GetComponentsInChildren<Dog> ()));
	}

	void Start () {
		startingPhase.TakeControl ();
	}

	void Update () {
		if (myAnimationManager.activelyAnimating) {
			myAnimationManager.AnimationUpdate ();
		}
		else if (inControl != null) {
			inControl.ControlUpdate ();
		}
	}

	// ---METHODS

	/// <summary>
	/// Helper method for GameControlPhase.TakeControl(). Kicks the previous phase out of control, calls its OnLeaveControl, then puts the new phase in charge and calls its OnTakeControl.
	/// </summary>
	public static void AssignControl (GameControlPhase phase) {
		if (inControl != null) {
			inControl.OnLeaveControl ();
		}
		phase.OnTakeControl ();
		inControl = phase;
	}

	// ---EVENT NOTIFIERS

	/// <summary>
	/// Calls the operating GameControlPhase's TileClickEvent().
	/// </summary>
	public static void RaiseTileClickEvent (Tile t) {
		if (inControl != null) {
			inControl.TileClickEvent (t);
		}
	}

	/// <summary>
	/// Calls the operating GameControlPhase's TileDoubleClickEvent().
	/// </summary>
	public static void RaiseTileDoubleClickEvent (Tile t) {
		if (inControl != null) {
			inControl.TileDoubleClickEvent (t);
		}
	}

	/// <summary>
	/// Calls the operating GameControlPhase's MouseOverChangeEvent().
	/// </summary>
	public static void RaiseMouseOverChangeEvent () {
		if (inControl != null) {
			inControl.MouseOverChangeEvent ();
		}
	}

	/// <summary>
	/// Calls the operating GameControlPhase's TileDragEvent().
	/// </summary>
	public static void RaiseTileDragEvent (Tile t) {
		if (inControl != null) {
			inControl.TileDragEvent (t);
		}
	}
		
	/// <summary>
	/// Calls the operating GameControlPhase's UICancelPathButtonEvent().
	/// </summary>
	public static void RaiseUICancelPathButtonEvent () {
		if (inControl != null) {
			inControl.UICancelPathButtonEvent ();
		}
	}
	/// <summary>
	/// Calls the operating GameControlPhase's UICancelPathButtonEvent().
	/// </summary>
	public static void InstanceRaiseUICancelPathButtonEvent () {
		GameBrain.RaiseUICancelPathButtonEvent ();
	}

	/// <summary>
	/// Calls the operating GameControlPhase's UIRestButtonEvent().
	/// </summary>
	public static void RaiseUIRestButtonEvent () {
		if (inControl != null) {
			inControl.UIRestButtonEvent ();
		}
	}
	/// <summary>
	/// Calls the operating GameControlPhase's UIRestButtonEvent().
	/// </summary>
	public void InstanceRaiseUIRestButtonEvent () {
		GameBrain.RaiseUIRestButtonEvent ();
	}

	/// <summary>
	/// Calls the operating GameControlPhase's UIActionButtonEvent().
	/// </summary>
	public static void RaiseUIActionButtonEvent () {
		if (inControl != null) {
			inControl.UIActionButtonEvent ();
		}
	}
	/// <summary>
	/// Calls the operating GameControlPhase's UIActionButtonEvent().
	/// </summary>
	public static void InstanceRaiseUIActionButtonEvent () {
		GameBrain.RaiseUIActionButtonEvent ();
	}
}
