using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the master text box that provides info on the selection.
/// </summary>
public class MasterInfoBox : MonoBehaviour {

	/// <summary>
	/// Title text on the box.
	/// </summary>
	public string title {
		get { return titleText.text; }
		set { titleText.text = value; }
	}

	[SerializeField] private Text titleText;


}
