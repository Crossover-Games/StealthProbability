using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Animates the transmission at the beginning of each level.
/// </summary>
public class StoryUIAnimator : MonoBehaviour {

	private enum ButtonConfigState {
		NextOnly, PrevAndNext, PrevAndEnd
	}

	private enum AnimationState {
		FadeIn, TextScroll, Idle, FadeOut
	}

	private RectTransform rectTransform;
	private ButtonConfigState m_buttonState = ButtonConfigState.NextOnly;
	private ButtonConfigState buttonState {
		get { return m_buttonState; }
		set {
			if (m_buttonState != value) {
				m_buttonState = value;
				switch (value) {
					case ButtonConfigState.NextOnly:
						nextBig.SetActive (true);
						nextSmall.SetActive (false);
						prevSmall.SetActive (false);
						endSmall.SetActive (false);
						break;
					case ButtonConfigState.PrevAndNext:
						nextBig.SetActive (false);
						nextSmall.SetActive (true);
						prevSmall.SetActive (true);
						endSmall.SetActive (false);
						break;
					case ButtonConfigState.PrevAndEnd:
						nextBig.SetActive (false);
						nextSmall.SetActive (false);
						prevSmall.SetActive (true);
						endSmall.SetActive (true);
						break;
				}
			}
		}
	}

	[SerializeField] private GameObject nextBig;
	[SerializeField] private GameObject nextSmall;
	[SerializeField] private GameObject prevSmall;
	[SerializeField] private GameObject endSmall;

	private AnimationState animState = AnimationState.FadeIn;
	[SerializeField] private Text textObject;

	private int messageIndex = 0;

	private Vector3 activePos;
	private Vector3 offscreenPos;

	/// <summary>
	/// In letters per second.
	/// </summary>
	[SerializeField] private float textFillRate;
	[SerializeField] private float fadeTime;
	private Timer myTimer;

	private string currentMessage {
		get { return TutorialTextHolder.messages [messageIndex]; }
	}

	void Awake () {
		rectTransform = GetComponent<RectTransform> ();
		activePos = rectTransform.anchoredPosition3D;
		offscreenPos = rectTransform.anchoredPosition3D + Vector3.right * 500f;
		rectTransform.anchoredPosition3D = offscreenPos;
		myTimer = new Timer (fadeTime);
	}

	void Start () {
		if (TutorialTextHolder.messages == null) {
			gameObject.SetActive (false);
		}
	}

	void Update () {
		switch (animState) {
			case AnimationState.FadeIn:
				myTimer.Tick ();
				rectTransform.anchoredPosition3D = Interpolation.Interpolate (rectTransform.anchoredPosition3D, activePos, myTimer.ratio, InterpolationMethod.SquareRoot);
				if (!myTimer.active) {
					BeginTextScroll ();
				}
				break;
			case AnimationState.TextScroll:
				myTimer.Tick ();
				textObject.text = currentMessage.Substring (0, Mathf.RoundToInt (currentMessage.Length * myTimer.ratio));
				if (!myTimer.active) {
					animState = AnimationState.Idle;
				}
				break;
			case AnimationState.FadeOut:
				myTimer.Tick ();
				rectTransform.anchoredPosition3D = Interpolation.Interpolate (rectTransform.anchoredPosition3D, offscreenPos, myTimer.ratio, InterpolationMethod.SquareRoot);
				if (!myTimer.active) {
					animState = AnimationState.TextScroll;
				}
				break;
		}
	}

	private void BeginTextScroll () {
		animState = AnimationState.TextScroll;
		myTimer = new Timer (currentMessage.Length / textFillRate);
	}

	[ContextMenu ("NEXT")]
	public void Next () {
		EventSystem.current.SetSelectedGameObject (null);
		if (myTimer.active) {
			myTimer.Disable ();
		}
		else {
			myTimer = new Timer (textFillRate);
			messageIndex++;
			if (messageIndex < TutorialTextHolder.messages.Length) {
				CalculateButtonState ();
				BeginTextScroll ();
			}
			else {
				animState = AnimationState.FadeOut;
			}
		}
	}

	[ContextMenu ("PREV")]
	public void Previous () {
		EventSystem.current.SetSelectedGameObject (null);
		myTimer = new Timer (currentMessage.Length / textFillRate);
		messageIndex--;
		CalculateButtonState ();
		BeginTextScroll ();
	}

	private void CalculateButtonState () {
		if (messageIndex == 0) {
			buttonState = ButtonConfigState.NextOnly;
		}
		else if (messageIndex == TutorialTextHolder.messages.Length - 1) {
			buttonState = ButtonConfigState.PrevAndEnd;
		}
		else {
			buttonState = ButtonConfigState.PrevAndNext;
		}
	}
}


