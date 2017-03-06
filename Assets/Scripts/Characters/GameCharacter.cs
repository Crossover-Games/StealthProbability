using UnityEngine;
using System.Collections;


/// <summary>
/// Represents a player unit or enemy guard.
/// </summary>
public abstract class GameCharacter : MonoBehaviour {
	[SerializeField] private Collider myCollider;

	/// <summary>
	/// Cat, dog, machine, or something else not thought of yet. It's safe to assume that a cat is implemented as a cat, and so on.
	/// </summary>
	virtual public CharacterType characterType {
		get{ return CharacterType.Machine; }
	}

	/// <summary>
	/// Encapsulated variable for the tile this character is standing on. Please don't modify this.
	/// </summary>
	[SerializeField] private Tile associatedTile;
	/// <summary>
	/// The tile this character is standing on.
	/// </summary>
	public Tile myTile {
		get{ return associatedTile; }
		set {
			associatedTile = value;
			associatedTile.SetOccupant (this);
		}
	}

	/// <summary>
	/// Compass direction this character is facing.
	/// </summary>
	public Compass.Direction orientation;

	/// <summary>
	/// Elevation of the top of the character's head in world space. Used for aligning the cursor.
	/// </summary>
	public float elevationOfTop {
		get {
			return myCollider.bounds.max.y;
		}
	}

	virtual protected void Awake () {
		FindMyStartingTile ();
	}

	/// <summary>
	/// Uses physics to determine what tile this character is standing on.
	/// </summary>
	private void FindMyStartingTile () {
		RaycastHit hit;
		Vector3 rayOrigin = transform.position + Vector3.down * 100;

		if (Physics.Raycast (rayOrigin, Vector3.up, out hit)) {
			Tile tileTemp = hit.collider.gameObject.GetComponent<Tile> ();
			if (tileTemp != null) {
				myTile = tileTemp;
			}
		}
	}

	/// <summary>
	/// PLACEHOLDER. Moves this character on top of the specified tile. Ideally, this would do some animation too, but that's not implemented yet.
	/// </summary>
	virtual public void MoveTo (Tile destination) {	//don't forget that this changes the tile's occupant
		if (destination != null) {
			Tile tmp = myTile;
			tmp.SetOccupant (null);

			myTile = destination;
			transform.position = myTile.characterConnectionPoint;
		}
	}
}
