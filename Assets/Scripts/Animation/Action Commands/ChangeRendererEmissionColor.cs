using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes the color of a renderer when executed.
/// </summary>
public class ChangeRendererEmissionColor : IActionCommand {
	private Renderer renderer;
	private Color color;
	public ChangeRendererEmissionColor (Renderer r, Color c) {
		renderer = r;
		color = c;
	}
	public void Execute () {
		Material m = renderer.material;
		m.EnableKeyword ("_EMISSION");
		m.SetColor ("_EmissionColor", color);
		renderer.material = m;
	}
}
