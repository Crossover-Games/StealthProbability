using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : GameCharacter {

	override public CharacterType characterType {
		get{ return CharacterType.Cat; }
	}

	override public float stepAnimationTime {
		get { return 0.5f; }
	}

	override public float stepRotationFraction {
		get { return 0.3f; }
	}

	[SerializeField] private int myMaxEnergy;
	/// <summary>
	/// The measure for how much a cat can do in a turn. Converts to movement, actions, and extra stealth.
	/// </summary>
	public int maxEnergy {
		get { return myMaxEnergy; }
	}

	/// <summary>
	/// Decreased by one on detection. If this hits zero, this cat is GONE
	/// </summary>
	public int livesRemaining = 2;

	private List<TileDangerData> dangerData = new List<TileDangerData> ();
	/// <summary>
	/// Registers a piece of tile danger data to this cat.
	/// </summary>
	public void RegisterDangerData (TileDangerData data) {
		dangerData.Add (data);
	}
	/// <summary>
	/// Clears all tile danger data for this cat.
	/// </summary>
	public void ClearDangerData () {
		dangerData = new List<TileDangerData> ();
	}

	/// <summary>
	/// All dogs who may have spotted this cat.
	/// </summary>
	public HashSet<Dog> dogsCrossed {
		get {
			HashSet<Dog> dogs = new HashSet<Dog> ();
			foreach (TileDangerData tdd in dangerData) {
				dogs.Add (tdd.watchingDog);
			}
			return dogs;
		}
	}

	/// <summary>
	/// Clears all danger data associated with a particular dog.
	/// </summary>
	public void ClearDangerByDog (Dog dog) {
		List<TileDangerData> danger2 = dangerData.Clone ();
		foreach (TileDangerData tdd in danger2) {
			if (tdd.watchingDog == dog) {
				dangerData.Remove (tdd);
			}
		}
	}

	/// <summary>
	/// Does this cat have any queued TileDangerDatas it needs resolved in a check?
	/// </summary>
	public bool inDanger {
		get { return dangerData.Count > 0; }
	}

	/// <summary>
	/// Runs a detection check by the specified dog. Returns true if the dog spotted the cat. Always false if the cat did not interfere with the vision pattern of the dog.
	/// </summary>
	public bool DetectionCheck (Dog watcher) {
		float maxDanger = -1f;
		foreach (TileDangerData tdd in dangerData) {
			if (tdd.watchingDog == watcher && tdd.danger > maxDanger) {
				maxDanger = tdd.danger;
			}
		}
		return Random.value < maxDanger;
	}

	override public void MoveTo (Floor destination) {
		if (Tile.IsValidMoveDestination (destination)) {
			foreach (TileDangerData tdd in destination.dangerData) {
				dangerData.Add (tdd);
			}
		}
		base.MoveTo (destination);
	}

	public bool walkingAnimation {
		get { return animator.GetBool ("Moving"); }
		set { animator.SetBool ("Moving", value); }
	}
}
