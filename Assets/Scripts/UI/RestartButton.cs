using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour {
	[ContextMenu ("Force Lose")]
	private void ForceLose () {
		LosePhase.TakeControl ();
	}

	private Vector3 defaultPos;
	private Vector3 offscreenPos;
	private RectTransform rect;
	[SerializeField] private float fadeTime;
	private Timer myTimer;

	void Awake () {
		rect = GetComponent<RectTransform> ();
		defaultPos = rect.anchoredPosition3D;
		offscreenPos = defaultPos - Vector3.up * 125f;
		rect.anchoredPosition3D = offscreenPos;
		myTimer = new Timer (fadeTime);
		myTimer.Disable ();
	}

	void Update () {
		if (myTimer.active) {
			myTimer.Tick ();
			rect.anchoredPosition3D = Interpolation.Interpolate (rect.anchoredPosition3D, defaultPos, myTimer.ratio, InterpolationMethod.SquareRoot);
		}
	}

	[ContextMenu ("Fade In")]
	public void BeginFadeIn () {
		myTimer.Restart ();
	}

	public void Click () {
		LoadLoadingScreen.SwitchToLoadingScreen ();
	}
}
