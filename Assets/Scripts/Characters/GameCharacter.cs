using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Represents a player unit or enemy guard.
/// </summary>
public abstract class GameCharacter : MonoBehaviour {

	[Tooltip ("This character's main sound")]
	[SerializeField]
	private AudioClip mySound;

	/// <summary>
	/// Used to play my sound.
	/// </summary>
	private AudioSource soundPlayer;

	/// <summary>
	/// Collider used to calculate the elevation of top.
	/// </summary>
	[SerializeField] private Collider myCollider;

	/// <summary>
	/// Cat, dog, machine, or something else not thought of yet. It's safe to assume that a cat is implemented as a cat, and so on.
	/// </summary>
	virtual public CharacterType characterType {
		get { return CharacterType.Machine; }
	}

	/// <summary>
	/// The amount of time it takes to step from one tile to the next.
	/// </summary>
	abstract public float stepAnimationTime { get; }
	/// <summary>
	/// The fraction of stepAnimationTime spent rotating. 
	/// </summary>
	abstract public float stepRotationFraction { get; }

	/// <summary>
	/// Encapsulated variable for myTile.
	/// </summary>
	private Tile associatedTile;
	/// <summary>
	/// The tile this character is standing on.
	/// </summary>
	public Tile myTile {
		get { return associatedTile; }
		set {
			associatedTile = value;
			associatedTile.SetOccupant (this);
		}
	}

	/// <summary>
	/// Plays this character's primary sound.
	/// </summary>
	public void PlaySound () {
		if (mySound != null) {
			soundPlayer.PlayOneShot (mySound);
		}
	}

	/// <summary>
	/// Compass direction this character is facing.
	/// </summary>
	public Compass.Direction orientation;

	protected Animator animator;

	private Renderer [] myRenderers;
	private Color myColor;

	/// <summary>
	/// Elevation of the top of the character's head in world space. Used for aligning the cursor.
	/// </summary>
	public float elevationOfTop {
		get {
			return myCollider.bounds.max.y;
		}
	}

	virtual protected void Awake () {
		soundPlayer = GetComponent<AudioSource> ();
		FindMyTile ();
		myRenderers = GetComponentsInChildren<Renderer> ();
		myColor = myRenderers [0].material.color;
		animator = GetComponentInChildren<Animator> ();
	}

	/// <summary>
	/// Uses physics to determine what tile this character is standing on.
	/// </summary>
	public void FindMyTile () {
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
	/// Moves this character on top of the specified tile. True if move was valid. This is intended to be used for neighboring tiles. There will be no animation if the destination is not a neighbor.
	/// </summary>
	virtual public void MoveTo (Tile destination) { //don't forget that this changes the tile's occupant
		if (Tile.ValidStepDestination (destination)) {
			Tile previous = myTile;
			previous.SetOccupant (null);
			myTile = destination;

			if (previous.IsNeighbor (myTile)) {
				Compass.Direction nextDirection = previous.GetDirectionOfNeighbor (myTile);
				float moveTime = stepAnimationTime;
				if (orientation != nextDirection) {
					orientation = nextDirection;
					moveTime = stepAnimationTime - stepAnimationTime * stepRotationFraction;
					AnimationManager.AddAnimation (transform, new AnimationDestination (null, Compass.DirectionToRotation (nextDirection), null, stepAnimationTime * stepRotationFraction, InterpolationMethod.Sinusoidal));
				}
				AnimationManager.AddAnimation (transform, new AnimationDestination (myTile.topCenterPoint, null, null, moveTime, InterpolationMethod.Sinusoidal));
			}
			else {
				transform.position = myTile.topCenterPoint;
			}
		}
	}

	private bool grayed = false;
	/// <summary>
	/// Is the character grayed out?
	/// </summary>
	public bool grayedOut {
		get { return grayed; }
		set {
			if (value != grayed) {
				grayed = value;
				foreach (Renderer r in myRenderers) {
					if (grayed) {
						r.material.color = Color.gray;
					}
					else {
						r.material.color = myColor;
					}
				}
			}
		}
	}
}
