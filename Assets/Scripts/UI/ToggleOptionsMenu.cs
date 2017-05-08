using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOptionsMenu : MonoBehaviour {
	[SerializeField] private GameObject optionsMenu;

	public void Click () {
		optionsMenu.SetActive (!optionsMenu.activeSelf);
	}

	public Text textObject;

	void Awake () {
		try {
			textObject.color = JsonUtility.FromJson<Color> (PlayerPrefs.GetString ("FavColor", JsonUtility.ToJson (Color.white)));
		}
		catch {
			textObject.color = Color.white;
		}
	}

	public void QuitGame () {
		Application.Quit ();
	}
}
