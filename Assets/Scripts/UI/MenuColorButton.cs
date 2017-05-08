using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuColorButton : MonoBehaviour {

	public Text textObject;

	public Color myColor;

	public void WriteColor () {
		print (myColor);
		textObject.color = myColor;
		PlayerPrefs.SetString ("FavColor", JsonUtility.ToJson (myColor));
	}
}
