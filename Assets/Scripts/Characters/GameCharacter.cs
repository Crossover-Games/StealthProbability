using UnityEngine;
using System.Collections;


/// <summary>
/// Represents a player unit or enemy guard.
/// </summary>
public abstract class GameCharacter : MonoBehaviour {
	/// <summary>
	/// Reference to the scene's GameBrain.
	/// </summary>
	private GameBrain brain;

	[SerializeField] private Collider myCollider;
	[SerializeField] private CharacterController myCharacterController;
	/// <summary>
	/// The character controller that moves this object.
	/// </summary>
	public CharacterController characterController {
		get{ return myCharacterController; }
	}

	/// <summary>
	/// Cat, dog, machine, or something else not thought of yet. It's safe to assume that a cat is implemented as a cat, and so on.
	/// </summary>
	virtual public CharacterType characterType {
		get{ return CharacterType.Machine; }
	}

	/// <summary>
	/// Encapsulated variable for myTile.
	/// </summary>
	private Tile associatedTile;
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
		brain = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameBrain> ();
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
	/// Moves this character on top of the specified tile. This is intended to be used for neighboring tiles. There will be no animation if the destination is not a neighbor.
	/// </summary>
	virtual public void MoveTo (Tile destination) {	//don't forget that this changes the tile's occupant
		if (destination != null) {
			Tile previous = myTile;
			previous.SetOccupant (null);
			myTile = destination;

			if (previous.IsNeighbor (myTile)) {
				brain.animationManager.StartAnimating (this, myTile.characterConnectionPoint, previous.GetDirectionOfNeighbor (myTile));
			}
			else {
				transform.position = myTile.characterConnectionPoint;
			}
		}
	}
}
