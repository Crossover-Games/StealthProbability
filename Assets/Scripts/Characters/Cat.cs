using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : GameCharacter {

	override public CharacterType characterType {
		get { return CharacterType.Cat; }
	}

	override public float stepAnimationTime {
		get { return 0.5f; }
	}

	override public float stepRotationFraction {
		get { return 0.3f; }
	}

	public override string flavorText {
		//get { return "Cat: Grunt unit of the international fashion police"; }
		get { return "Cat: Feline freedom fighter"; }
	}

	[SerializeField] private GameObject wetEffect;

	[SerializeField] private int m_maxEnergy;
	/// <summary>
	/// The measure for how much a cat can do in a turn. Converts to movement and extra stealth.
	/// </summary>
	public int maxEnergy {
		get { return m_maxEnergy; }
	}

	private static string defaultColorJson {
		get { return "{\"r\":0.5,\"g\":0.0,\"b\":1.0,\"a\":1.0}"; }
	}

	override protected void Awake () {
		base.Awake ();
		try {
			defaultColor = JsonUtility.FromJson<Color> (PlayerPrefs.GetString ("FavColor", defaultColorJson));
		}
		catch {
			defaultColor = JsonUtility.FromJson<Color> (defaultColorJson);
		}
	}

	void Start () {
		grayedOut = true;
		grayedOut = false;
	}

	public int stealthStacks = 0;

	/// <summary>
	/// Evade detection once.
	/// </summary>
	public bool hasWildCard = true;

	public void Soak () {
		wetEffect.SetActive (true);
		m_wetTurns = maxWetTurns;
	}

	/// <summary>
	/// Inverts the probabilities
	/// </summary>
	public bool isWet {
		get { return m_wetTurns > 0; }
	}

	/// <summary>
	/// Total number of turns a soaked cat is wet. Effectively, it will be one lower than this.
	/// </summary>
	private static int maxWetTurns {
		get { return 4; }
	}
	private int m_wetTurns = 0;
	/// <summary>
	/// Turns the cat remains wet
	/// </summary>
	public int wetTurnsRemaining {
		get { return m_wetTurns; }
	}

	public void DecrementWetTurns () {
		if (isWet) {
			m_wetTurns--;
			if (!isWet) {
				wetEffect.SetActive (false);
			}
		}
	}
	/// <summary>
	/// Moves and gathers danger.
	/// </summary>
	override public void MoveTo (Tile destination) {
		bool valid = Tile.ValidStepDestination (destination);
		if (valid) {
			if (destination.occupant != null) {
				CatExecutePhase.charactersCrossed.Push (destination.occupant);
			}
		}
		base.MoveTo (destination);
		if (valid) {
			TileDangerData [] dangerArray = myTile.dangerData;
			if (dangerArray.Length > 0) {
				DetectionManager.AddDanger (this, dangerArray);
			}
			if (destination.tileType == TileType.WetFloor && (destination as WetFloor).active) {
				Soak ();
			}
		}
	}

	public bool walkingAnimation {
		get { return animator.GetBool ("Moving"); }
		set { animator.SetBool ("Moving", value); }
	}
}
