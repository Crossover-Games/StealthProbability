using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : Dog {
	override public CharacterType characterType {
		get { return CharacterType.Machine; }
	}

	override protected void Awake () {
		SetUpReferencesOnAwake ();
		m_VisionPattern = VisionPattern.VisionPatternFromType (this, DogVisionPatternType.Beacon);
	}

	override protected void Start () { }

	public override string flavorText { get { return "joycon boyz"; } }

	public override float stepAnimationTime { get { return 0f; } }

	public override float stepRotationFraction { get { return 0f; } }
}
