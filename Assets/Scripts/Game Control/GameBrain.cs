using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the flow of what is allowed to happen in the game. Allows one GameControlPhase to operate at a time, and allows switching between them.
/// </summary>
public class GameBrain : MonoBehaviour {

	[Tooltip ("The first phase. Its OnTakeControl will be called.")]
	[SerializeField] private GameControlPhase startingPhase;

	/// <summary>
	/// The phase that is currently controlling the game.
	/// </summary>
	private static GameControlPhase inControl = null;

	// ---REFERENCES

	private static TeamMananger<Cat> m_catManager;
	/// <summary>
	/// Has knowledge of all cats.
	/// </summary>
	public static TeamMananger<Cat> catManager {
		get { return m_catManager; }
	}

	private static TeamMananger<Dog> m_dogManager;
	/// <summary>
	/// Has knowledge of all dogs.
	/// </summary>
	public static TeamMananger<Dog> dogManager {
		get { return m_dogManager; }
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
		if (inControl != null && !AnimationManager.active) {
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
	public void InstanceRaiseUICancelPathButtonEvent () {
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
