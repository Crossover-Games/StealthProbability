using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Button that appears when you win the level.
/// </summary>
public class NextLevelButton : MonoBehaviour {

	[ContextMenu ("Force Victory")]
	private void ForceWin () {
		VictoryPhase.TakeControl ();
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
