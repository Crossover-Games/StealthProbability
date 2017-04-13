using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDangerVisualizer : MonoBehaviour {
	public static Color RED {
		get { return new Color (1f, 0f, 0f, 1f); }
	}
	public static Color YELLOW {
		get { return new Color (1f, 1f, 0f, 1f); }
	}
	public static Color GREEN {
		get { return new Color (0f, 1f, 0f, 1f); }
	}
	public static Color WHITE {
		get { return new Color (1f, 1f, 1f, 1f); }
	}
	public static Color CAROLINA_BLUE {
		get { return new Color (123f / 255f, 175f / 255f, 212f / 255f, 1f); }
	}

	public static Color RandomColor () {
		return Random.ColorHSV (0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
	}

	private Renderer m_Renderer;
	[SerializeField] private SpriteRenderer m_SpriteRenderer;
	//[SerializeField] private GameObject sparks;

	void Awake () {
		m_Renderer = GetComponent<Renderer> ();
		gameObject.SetActive (false);
	}

	/// <summary>
	/// This tile's base danger color.
	/// </summary>
	public Color tileDangerColor {
		get { return m_Renderer.material.color; }
		set {
			m_Renderer.material.color = value;
		}
	}

	/// <summary>
	/// This tile's hologram danger color.
	/// </summary>
	public Color hologramColor {
		get { return m_SpriteRenderer.color; }
		set {
			m_SpriteRenderer.color = value.AlphaDifferent (0.75f);
		}
	}
}
