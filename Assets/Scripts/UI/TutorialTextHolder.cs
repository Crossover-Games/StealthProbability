using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Carries the tutorial text for the level.
/// </summary>
public class TutorialTextHolder : MonoBehaviour {
	[TextArea]
	[SerializeField]
	private string [] m_messages;

	private static TutorialTextHolder staticInstance = null;

	public static string [] messages {
		get {
			if (staticInstance == null) {
				return null;
			}
			else {
				return staticInstance.m_messages;
			}
		}
	}

	void Awake () {
		staticInstance = this;
	}
}
