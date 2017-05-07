using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStart : MonoBehaviour {
	public void Click () {
		LoadLoadingScreen.PrimeNextLoadingScreen ();
		LoadLoadingScreen.SwitchToLoadingScreen ();
	}
}
