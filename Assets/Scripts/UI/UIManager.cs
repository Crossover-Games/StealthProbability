using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls various aspects of the UI. Largely undeveloped currently.
/// </summary>
public class UIManager : MonoBehaviour {
	// control the thing that

	[SerializeField] private GameBrain brain;
	[SerializeField] private Canvas canvas;
	private RectTransform canvasRect;

	[SerializeField] private RectTransform pathEndMenu;

	private Vector3Reference followPoint = null;

	void Awake () {
		canvasRect = canvas.GetComponent<RectTransform> ();
	}

	void LateUpdate () {
		if (pathEndMenu.gameObject.activeSelf && followPoint != null) {
			PathEndMenuTracking ();
		}
	}

	/// <summary>
	/// State of the path end menu.
	/// </summary>
	public bool pathEndMenuState {
		get { return pathEndMenu.gameObject.activeSelf; }
		set { pathEndMenu.gameObject.SetActive (value); }
	}

	/// <summary>
	/// Centers the path end menu on the mouse position.
	/// </summary>
	public void CenterPathEndMenuOnMouse () {
		Vector3 screenPoint = Input.mousePosition;
		screenPoint.z = canvas.planeDistance;
		pathEndMenu.position = canvas.worldCamera.ScreenToWorldPoint (screenPoint);
	}

	/// <summary>
	/// Centers the path end menu on a specific world point. 
	/// </summary>
	public void CenterPathEndMenuOnWorldPoint (Vector3 point) {
		followPoint = new Vector3Reference (point);
	}

	private void PathEndMenuTracking () {
		Vector2 viewportPosition = canvas.worldCamera.WorldToViewportPoint (followPoint.vector);
		Vector2 WorldObject_ScreenPosition = new Vector2 (
			                                     ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
			                                     ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
		pathEndMenu.anchoredPosition = WorldObject_ScreenPosition;
	}

	//public void
}
