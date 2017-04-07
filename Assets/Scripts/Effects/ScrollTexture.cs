using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour {

	[SerializeField] private float scrollSpeed = 1f;
	private float offset {
		get{ return Time.timeSinceLevelLoad * scrollSpeed / 10.0f; }
	}
	//[SerializeField] private float rotate = 0.5f;

	private Renderer myRenderer;

	void Awake () {
		myRenderer = GetComponent<Renderer> ();
	}

	void Update () {
		//offset += (Time.deltaTime * scrollSpeed) / 10.0f;
		//offset = Time.timeSinceLevelLoad / 10.0f;
		myRenderer.material.SetTextureOffset ("_MainTex", new Vector2 (offset, offset));
	}
}
