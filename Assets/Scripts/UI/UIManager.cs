using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls various aspects of the UI. Largely undeveloped currently.
/// </summary>
public class UIManager : MonoBehaviour {

	[SerializeField] private Canvas canvasInstance;
	/// <summary>
	/// The UI canvas.
	/// </summary>
	private static Canvas canvas;
	/// <summary>
	/// RectTransform associated with the UI canvas.
	/// </summary>
	private static RectTransform canvasRect;

	[SerializeField] private RectTransform pathEndMenuInstance;
	/// <summary>
	/// The path end menu.
	/// </summary>
	private static RectTransform pathEndMenu;

	private static Vector3? followPoint = null;


	[SerializeField] private MasterInfoBox infoBoxInstance;
	private static MasterInfoBox m_infoBox;
	/// <summary>
	/// Controls the master text box that provides info on the selection.
	/// </summary>
	public static MasterInfoBox masterInfoBox {
		get { return m_infoBox; }
	}

	private static Route m_currentlyDisplayed = null;
	/// <summary>
	/// The current route being displayed.
	/// </summary>
	public static Route routeCurrentlyDisplayed {
		get { return m_currentlyDisplayed; }
		set {
			if (m_currentlyDisplayed != value) {
				if (m_currentlyDisplayed != null) {
					m_currentlyDisplayed.visualState = false;
				}
				m_currentlyDisplayed = value;
				if (m_currentlyDisplayed != null) {
					m_currentlyDisplayed.visualState = true;
				}
			}
		}
	}

	void Awake () {
		m_infoBox = infoBoxInstance;
		canvas = canvasInstance;
		canvasRect = canvasInstance.GetComponent<RectTransform> ();
		pathEndMenu = pathEndMenuInstance;
	}

	void LateUpdate () {
		if (pathEndMenu.gameObject.activeSelf && followPoint != null) {
			PathEndMenuTracking ();
		}
	}

	/// <summary>
	/// State of the path end menu.
	/// </summary>
	public static bool pathEndMenuState {
		get { return pathEndMenu.gameObject.activeSelf; }
		set { pathEndMenu.gameObject.SetActive (value); }
	}

	/// <summary>
	/// Centers the path end menu on the mouse position.
	/// </summary>
	public static void CenterPathEndMenuOnMouse () {
		Vector3 screenPoint = Input.mousePosition;
		screenPoint.z = canvas.planeDistance;
		pathEndMenu.position = canvas.worldCamera.ScreenToWorldPoint (screenPoint);
	}

	/// <summary>
	/// Centers the path end menu on a specific world point. 
	/// </summary>
	public static void CenterPathEndMenuOnWorldPoint (Vector3 point) {
		followPoint = point;
	}

	private static void PathEndMenuTracking () {
		Vector2 viewportPosition = canvas.worldCamera.WorldToViewportPoint (followPoint.GetValueOrDefault ());
		Vector2 WorldObject_ScreenPosition = new Vector2 (
												 ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
												 ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
		pathEndMenu.anchoredPosition = WorldObject_ScreenPosition;
	}
}
