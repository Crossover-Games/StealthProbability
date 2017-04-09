﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Path arrow drawn in DrawPathArrowPhase
/// </summary>
public class PathArrow : MonoBehaviour {
	[SerializeField] private GameObject [] segments;
	/// <summary>
	/// Pool of objects used to draw the arrow. Be mindful of the max.
	/// </summary>
	public GameObject [] lineSegments {
		get { return segments; }
	}

	private Renderer [] allRenderers;

	[SerializeField] private Material arrowMaterial;
	/// <summary>
	/// The color of the arrow.
	/// </summary>
	public Color color {
		get { return allRenderers [0].material.color; }
		set {
			Material m = new Material (arrowMaterial);
			m.color = value;
			foreach (Renderer r in allRenderers) {
				//r.SetMainMaterialColor (value);
				r.material = m;
			}
		}
	}
	void Awake () {
		allRenderers = new Renderer [lineSegments.Length];
		for (int x = 0; x < allRenderers.Length; x++) {
			allRenderers [x] = lineSegments [x].GetComponentInChildrenUnconditional<Renderer> ();
		}
	}

	/// <summary>
	/// Turns off the arrow.
	/// </summary>
	public void ClearArrow () {
		foreach (GameObject go in segments) {
			go.SetActive (false);
		}
	}
}
