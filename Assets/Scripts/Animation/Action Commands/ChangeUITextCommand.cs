using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextMeshCommand : IActionCommand {

	private TextMesh textObject;
	private string value;
	private Color? color;

	public ChangeTextMeshCommand (TextMesh textObject, string value, Color? color = null) {
		this.textObject = textObject;
		this.value = value;
		this.color = color;
	}

	public void Execute () {
		textObject.text = value;
		if (color != null) {
			textObject.color = color.GetValueOrDefault ();
		}
	}
}
