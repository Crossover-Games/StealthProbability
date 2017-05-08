using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the flow of what is allowed to happen in the game. Allows one GameControlPhase to operate at a time, and allows switching between them.
/// </summary>
public class GameBrain : MonoBehaviour {

	[Tooltip ("The first phase. Its OnTakeControl will be called.")]
	[SerializeField]
	private GameControlPhase startingPhase;

	/// <summary>
	/// The phase that is currently controlling the game.
	/// </summary>
	private static GameControlPhase inControl = null;

	// ---REFERENCES

	private static CatManager m_catManager;
	/// <summary>
	/// Has knowledge of all cats.
	/// </summary>
	public static CatManager catManager {
		get { return m_catManager; }
	}

	private static TeamMananger<Dog> m_dogManager;
	/// <summary>
	/// Has knowledge of all dogs.
	/// </summary>
	public static TeamMananger<Dog> dogManager {
		get { return m_dogManager; }
	}

	private static TeamMananger<Dog> m_machineManager;
	/// <summary>
	/// Has knowledge of all dog-aligned machines.
	/// </summary>
	public static TeamMananger<Dog> machineManager {
		get { return m_machineManager; }
	}


	void Awake () {
		Transform charactersParent = GameObject.FindGameObjectWithTag ("CharactersParent").transform;
		m_catManager = new CatManager (new List<Cat> (charactersParent.Find ("Cat Parent").GetComponentsInChildren<Cat> ()));
		m_dogManager = new TeamMananger<Dog> (new List<Dog> (charactersParent.Find ("Dog Parent").GetComponentsInChildren<Dog> ()));
		m_machineManager = new TeamMananger<Dog> (new List<Dog> (charactersParent.Find ("Machine Parent").GetComponentsInChildren<Dog> ()));
	}

	void Start () {
		startingPhase.InstanceTakeControl ();
	}

	void Update () {
		if (inControl != null && !AnimationManager.active) {
			inControl.ControlUpdate ();
		}
		inControl.StandardUpdate ();
		if (Input.GetKeyDown (KeyCode.LeftShift) && Input.GetKey (KeyCode.Escape) || Input.GetKeyDown (KeyCode.Escape) && Input.GetKey (KeyCode.LeftShift)) {
			MainMenuShortcut.ToMainMenu ();
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
	/// Calls the operating GameControlPhase's UIStayButtonEvent().
	/// </summary>
	public static void RaiseUIStayButtonEvent () {
		if (inControl != null) {
			inControl.UIStayButtonEvent ();
		}
	}
	/// <summary>
	/// Calls the operating GameControlPhase's UIStayButtonEvent().
	/// </summary>
	public void InstanceRaiseUIStayButtonEvent () {
		GameBrain.RaiseUIStayButtonEvent ();
	}
}
