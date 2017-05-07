using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Dog {

	override public CharacterType characterType {
		get { return CharacterType.Machine; }
	}

	[SerializeField] private float m_probability;
	public float probability { get { return m_probability; } }

	override protected void Awake () {
		SetUpReferencesOnAwake ();
		m_VisionPattern = new LaserVisionPattern (this);
	}

	override protected void Start () { }

	//public override string flavorText { get { return "Light Amplification by Stimulated Emission of Radiation"; } }
	public override string flavorText { get { return "Security laser: Immobile cat infiltration countermeasure"; } }

	public override float stepAnimationTime { get { return 0f; } }

	public override float stepRotationFraction { get { return 0f; } }
}
