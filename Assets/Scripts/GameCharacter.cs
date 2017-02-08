using UnityEngine;
using System.Collections;


/// <summary>
/// PLACEHOLDER. Represents a player unit or enemy guard.
/// </summary>
public class GameCharacter : MonoBehaviour {
	public Collider myCollider;
	public float elevationOfTop {
		get {
			return myCollider.bounds.max.y;
		}
	}
}
