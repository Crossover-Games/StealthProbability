using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerSquareVisualizer : MonoBehaviour {
	public static Color RED {
		get{ return new Color (1f, 0f, 0f, 1f); }
	}
	public static Color YELLOW {
		get{ return new Color (1f, 1f, 0f, 1f); }
	}
	public static Color GREEN {
		get{ return new Color (0f, 1f, 0f, 1f); }
	}
	public static Color WHITE {
		get{ return new Color (1f, 1f, 1f, 1f); }
	}
	public static Color CAROLINA_BLUE {
		get{ return new Color (123f/255f, 175f/255f, 212f/255f, 1f); }
	}

	public static Color RandomColor () {
		return Random.ColorHSV (0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
	}

	private Renderer myRenderer;

	void Awake () {
		myRenderer = GetComponent<Renderer> ();
		gameObject.SetActive (false);
	}

	public Color color {
		get{ return myRenderer.material.color; }
		set{ myRenderer.material.color = value; }
	}
}
