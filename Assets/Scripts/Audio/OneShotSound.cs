using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotSound : MonoBehaviour {
	[SerializeField]
	private AudioClip mySound;

	/// <summary>
	/// Used to play my sound.
	/// </summary>
	private AudioSource soundPlayer;

	void Awake () {
		soundPlayer = GetComponent<AudioSource> ();
	}

	public void PlaySound () {
		if (mySound != null) {
			soundPlayer.PlayOneShot (mySound);
		}
		else {
			print ("no sound!");
		}
	}
}
