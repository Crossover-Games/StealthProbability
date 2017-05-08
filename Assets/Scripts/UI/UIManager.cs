using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Controls various aspects of the UI. Largely undeveloped currently.
/// </summary>
public class UIManager : MonoBehaviour {

	[SerializeField] private Image restBG;
	[SerializeField] private Image cancelBG;
	[SerializeField] private Image stayBG;
	private static Image restBG_s;
	private static Image cancelBG_s;
	private static Image stayBG_s;
	private static Color restColor;
	private static Color cancelColor;
	private static Color stayColor;

	private static void ResetColors () {
		restBG_s.color = restColor;
		cancelBG_s.color = cancelColor;
		stayBG_s.color = stayColor;
	}

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

	[SerializeField] private RectTransform stayMenuInstance;
	/// <summary>
	/// The stay menu.
	/// </summary>
	private static RectTransform stayMenu;

	private static Vector3? followPoint = null;


	private static bool mouseStay = false;
	public static bool mouseOverStayMenu {
		get { return mouseStay && stayMenu.gameObject.activeSelf; }
	}
	public void RegisterMouseStay () {
		mouseStay = true;
	}
	public void UnregisterMouseStay () {
		mouseStay = false;
	}

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
		restColor = restBG.color;
		cancelColor = cancelBG.color;
		stayColor = stayBG.color;

		restBG_s = restBG;
		cancelBG_s = cancelBG;
		stayBG_s = stayBG;

		m_infoBox = infoBoxInstance;
		canvas = canvasInstance;
		canvasRect = canvasInstance.GetComponent<RectTransform> ();
		pathEndMenu = pathEndMenuInstance;
		stayMenu = stayMenuInstance;
	}

	void LateUpdate () {
		if (followPoint != null) {
			if (pathEndMenu.gameObject.activeSelf) {
				PathEndMenuTracking ();
			}
			else if (stayMenu.gameObject.activeSelf) {
				StayMenuTracking ();
			}
		}
	}

	/// <summary>
	/// State of the path end menu.
	/// </summary>
	public static bool pathEndMenuState {
		get { return pathEndMenu.gameObject.activeSelf; }
		set {
			EventSystem.current.SetSelectedGameObject (null);
			ResetColors ();
			pathEndMenu.gameObject.SetActive (value);
		}
	}

	/// <summary>
	/// State of the path end menu.
	/// </summary>
	public static bool stayMenuState {
		get { return stayMenu.gameObject.activeSelf; }
		set {
			EventSystem.current.SetSelectedGameObject (null);
			ResetColors ();
			if (!value) {
				mouseStay = false;
			}
			stayMenu.gameObject.SetActive (value);
		}
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
	public static void CenterMenusOnWorldPoint (Vector3 point) {
		followPoint = point;
	}


	private static void PathEndMenuTracking () {
		Vector2 viewportPosition = canvas.worldCamera.WorldToViewportPoint (followPoint.GetValueOrDefault ());
		Vector2 WorldObject_ScreenPosition = new Vector2 (
												 ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
												 ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
		pathEndMenu.anchoredPosition = WorldObject_ScreenPosition;
	}

	private static void StayMenuTracking () {
		Vector2 viewportPosition = canvas.worldCamera.WorldToViewportPoint (followPoint.GetValueOrDefault ());
		Vector2 WorldObject_ScreenPosition = new Vector2 (
												 ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
												 ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
		stayMenu.anchoredPosition = WorldObject_ScreenPosition;
	}
}
