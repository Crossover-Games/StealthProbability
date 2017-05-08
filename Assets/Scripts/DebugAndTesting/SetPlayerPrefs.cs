using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerPrefs : MonoBehaviour {

	public string key;
	public Color value;

	[ContextMenu ("SetColor")]
	public void SetColor () {
		PlayerPrefs.SetString (key, JsonUtility.ToJson (value));
	}

	[ContextMenu ("DeleteAll")]
	public void DeleteAll () {
		PlayerPrefs.DeleteAll ();
	}
}
