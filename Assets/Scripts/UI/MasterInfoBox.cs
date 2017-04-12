using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the master text box that provides info on the selection.
/// </summary>
public class MasterInfoBox : MonoBehaviour {

	/// <summary>
	/// Header text on the box.
	/// </summary>
	public string headerText {
		get { return titleText.text; }
		set { titleText.text = value; }
	}

	[SerializeField] private Text titleText;

	private int numberOfDataEnabled = 0;
	/// <summary>
	/// All fields of text data. Assign in editor.
	/// </summary>
	[SerializeField] private Text [] dataFields;

	/// <summary>
	/// Adds one piece of text data.
	/// </summary>
	public void AddData (string info, Color color) {
		if (numberOfDataEnabled < dataFields.Length) {
			dataFields [numberOfDataEnabled].text = info;
			dataFields [numberOfDataEnabled].color = color;
			dataFields [numberOfDataEnabled].gameObject.SetActive (true);
			numberOfDataEnabled++;
		}
	}

	/// <summary>
	/// Adds the data from tile danger data using a convention.
	/// </summary>
	public void AddDataFromTileDangerData (TileDangerData data) {
		AddData ("* " + Mathf.FloorToInt (data.danger * 100).ToString () + "% from " + data.watchingDog.name, data.dangerColor.OptimizedForText ());
	}

	/// <summary>
	/// Clears all text data. Does not affect the header box.
	/// </summary>
	public void ClearAllData () {
		numberOfDataEnabled = 0;
		for (int x = 0; x < dataFields.Length; x++) {
			dataFields [x].gameObject.SetActive (false);
		}
	}
}
