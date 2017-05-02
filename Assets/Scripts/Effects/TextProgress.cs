using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextProgress : MonoBehaviour {
	private Text textObject;
	private string defaultText;

	[SerializeField] private float completionTime;

	private Timer myTimer;

	void Awake () {
		textObject = GetComponent<Text> ();
		defaultText = textObject.text;
		myTimer = new Timer (completionTime);
	}

	void Update () {
		if (myTimer.active) {
			myTimer.Tick ();
			textObject.text = defaultText.Substring (0, Mathf.RoundToInt (defaultText.Length * myTimer.ratio));
		}
		else {
			enabled = false;
		}
	}
}
