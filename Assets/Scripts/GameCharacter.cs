using UnityEngine;
using System.Collections;


/// <summary>
/// PLACEHOLDER. Represents a player unit or enemy guard.
/// </summary>
public abstract class GameCharacter : MonoBehaviour {
	[SerializeField] private Collider myCollider;
	public float elevationOfTop {
		get {
			return myCollider.bounds.max.y;
		}
	}
}
