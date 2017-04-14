using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEditorControls : MonoBehaviour {
	[ContextMenu ("Custom Scale (One Shot)")]
	public void CustomScale () {
		// Scale all the particle components based on parent.
		ParticleSystem [] particles = gameObject.GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem particle in particles) {
			particle.startSize *= gameObject.transform.localScale.Average ();
			//particle.startLifetime *= gameObject.transform.localScale.Average ();
			ParticleSystemRenderer renderer = particle.GetComponent<ParticleSystemRenderer> ();
			if (renderer) {
				renderer.lengthScale *= gameObject.transform.localScale.Average ();
				renderer.velocityScale *= gameObject.transform.localScale.Average ();
			}
		}
	}
	[ContextMenu ("Prewarm All")]
	public void PrewarmAll () {
		ParticleSystem [] particles = gameObject.GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem particle in particles) {
			ParticleSystem.MainModule main = particle.main;
			main.prewarm = true;
		}
	}
}
