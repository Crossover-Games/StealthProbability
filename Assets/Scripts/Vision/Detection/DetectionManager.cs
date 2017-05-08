using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton for managing detection of cats.
/// </summary>
public class DetectionManager : MonoBehaviour {
	[SerializeField] private GameObject highlightA;
	[SerializeField] private GameObject highlightB;

	private static DetectionManager staticInstance;

	/// <summary>
	/// Maps cats to the danger they have each accumulated.
	/// </summary>
	private Dictionary<Cat, List<TileDangerData>> danger = new Dictionary<Cat, List<TileDangerData>> ();

	void OnDrawGizmos () {
		foreach (KeyValuePair<Cat, List<TileDangerData>> kp in danger) {
			Gizmos.DrawSphere (kp.Key.transform.position, 0.6f);
		}
	}

	void Awake () {
		staticInstance = this;
	}

	/// <summary>
	/// Plays a one shot particle effect.
	/// </summary>
	public static void SetConflictHighlight (DetectionMatchup matchup) {
		staticInstance.highlightA.SetActive (false);
		staticInstance.highlightB.SetActive (false);
		staticInstance.highlightA.transform.position = matchup.catInDanger.myTile.cursorConnectionPoint;
		staticInstance.highlightB.transform.position = matchup.watchingDog.myTile.cursorConnectionPoint;
		staticInstance.highlightA.SetActive (true);
		staticInstance.highlightB.SetActive (true);
	}

	/// <summary>
	/// The number of cats currently in danger.
	/// </summary>
	public static int catsInDanger {
		get { return staticInstance.danger.Count; }
	}

	/// <summary>
	/// Register the danger under all cats.
	/// </summary>
	public static void GatherDanger () {
		foreach (Cat c in GameBrain.catManager.allCharacters) {
			TileDangerData [] allDangerHere = c.myTile.dangerData;
			if (allDangerHere.Length > 0) {
				if (staticInstance.danger.ContainsKey (c)) {
					staticInstance.danger [c].AddRange (allDangerHere);
				}
				else {
					staticInstance.danger.Add (c, new List<TileDangerData> (allDangerHere));
				}
			}
		}
	}

	/// <summary>
	/// Register a tile danger data to a specified cat.
	/// </summary>
	public static void AddDanger (Cat c, TileDangerData tdd) {
		if (staticInstance.danger.ContainsKey (c)) {
			staticInstance.danger [c].Add (tdd);
		}
		else {
			List<TileDangerData> temp = new List<TileDangerData> (1);
			temp.Add (tdd);
			staticInstance.danger.Add (c, temp);
		}
	}

	/// <summary>
	/// Register a collection of tile danger data to a specified cat.
	/// </summary>
	public static void AddDanger (Cat c, IEnumerable<TileDangerData> dangerList) {
		if (staticInstance.danger.ContainsKey (c)) {
			staticInstance.danger [c].AddRange (dangerList);
		}
		else {
			List<TileDangerData> temp = new List<TileDangerData> (1);
			temp.AddRange (dangerList);
			staticInstance.danger.Add (c, temp);
		}
	}

	/// <summary>
	/// Clear all danger from all cats.
	/// </summary>
	public static void ClearAllDanger () {
		staticInstance.danger = new Dictionary<Cat, List<TileDangerData>> ();
	}

	/// <summary>
	/// Reveal all checks the first cat must make. This removes them from the DetectionManager.
	/// </summary>
	private static Queue<DetectionMatchup> FirstCatAllChecks () {
		Queue<DetectionMatchup> checks = new Queue<DetectionMatchup> ();
		if (catsInDanger > 0) {
			KeyValuePair<Cat, List<TileDangerData>> temp = staticInstance.danger.FirstElement ();
			Cat c = temp.Key;
			List<TileDangerData> catDanger = temp.Value;
			staticInstance.danger.Remove (c);

			Dictionary<Dog, float> riskiestPerDog = new Dictionary<Dog, float> ();
			foreach (TileDangerData tdd in catDanger) {
				if (!riskiestPerDog.ContainsKey (tdd.watchingDog)) {
					riskiestPerDog.Add (tdd.watchingDog, tdd.danger);
				}
				else if (tdd.danger > riskiestPerDog [tdd.watchingDog]) {
					riskiestPerDog [tdd.watchingDog] = tdd.danger;
				}
			}
			foreach (KeyValuePair<Dog, float> kp in riskiestPerDog) {
				checks.Enqueue (new DetectionMatchup (c, kp.Key, kp.Value));
			}
		}
		return checks;
	}

	public static void ClearDangerByCat (Cat c) {
		if (staticInstance.danger.ContainsKey (c)) {
			staticInstance.danger.Remove (c);
		}
	}

	/// <summary>
	/// Returns all checks that must be made for all cats. There should be no pending danger after this.
	/// </summary>
	public static Queue<DetectionMatchup> AllChecks () {
		Queue<DetectionMatchup> checks = new Queue<DetectionMatchup> ();
		while (catsInDanger > 0) {
			checks.EnqueueRange (FirstCatAllChecks ());
		}
		return checks;
	}
}
