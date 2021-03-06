﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Animates three dots after a text field.
/// </summary>
public class TextDotDotDot : MonoBehaviour {

	[SerializeField] private Text myText;
	[SerializeField] private float cycleDuration;

	[Tooltip ("If true, dots are placed at the front. If false, placed at back")]
	[SerializeField]
	private bool placeAtFront;

	private string defaultText;
	private Timer myTimer;

	void Awake () {
		myTimer = new Timer (cycleDuration);
		defaultText = myText.text;
	}

	void Update () {
		string dots = "";
		for (int x = 0; x < Utility.Partition (myTimer.ratio, 1f, 4); x++) {
			dots += ".";
		}
		if (placeAtFront) {
			myText.text = dots + defaultText;
		}
		else {
			myText.text = defaultText + dots;
		}
		myTimer.Tick ();
		if (!myTimer.active) {
			myTimer.Restart ();
		}
	}
}

