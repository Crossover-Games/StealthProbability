using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// One whole path a dog can move along, segmented into individual routes.
/// </summary>
public class Route : MonoBehaviour {

	/// <summary>
	/// Registers a path to this route. Setup only. For visualizer.
	/// </summary>
	public void AddPath (Path p) {
		p.SetSerializedRoute (this);
		allPaths.Add (p);
	}
	[SerializeField] private List<Path> allPaths; // only for visualization

	/// <summary>
	/// Editor only. Do not call at runtime.
	/// </summary>
	public void SetSerializedDog (Dog d) {
		m_dog = d;
	}
	[SerializeField] private Dog m_dog;
	/// <summary>
	/// The dog who uses this path.
	/// </summary>
	public Dog myDog {
		get { return m_dog; }
	}

	/// <summary>
	/// The last path this dog took. This is so the dog doesn't backtrack.
	/// </summary>
	public Path pathLastTaken = null;

	/// <summary>
	/// Dogs typically have a predetermined path they take on their first turn. When this is not null, the dog always takes this route.
	/// </summary>
	private Path firstPath = null;

	/// <summary>
	/// The dog will definitely choose this path on the next choice it has to make. Use this to set the guaranteed first route.
	/// </summary>
	public void SetGuaranteedPath (Path p) {
		firstPath = p;
	}

	/// <summary>
	/// Returns all possible paths for the dog on its next turn.
	/// </summary>
	public List<Path> immediateChoicesForDog {
		get {
			if (firstPath != null) {
				List<Path> temp = new List<Path> ();
				temp.Add (firstPath);
				return temp;
			}
			else {
				HashSet<Path> temp = new HashSet<Path> ();
				foreach (StepNode sn in m_dog.myTile.stepNode.connections) {
					if (sn.myPath != pathLastTaken) {
						temp.Add (sn.myPath);
					}
				}

				return temp.ToList ();
			}
		}
	}

	/// <summary>
	/// The parent to all path visualizers on this route.
	/// </summary>
	public GameObject visualizerParent;
	/// <summary>
	/// Display this route?
	/// </summary>
	public bool visualState {
		get { return visualizerParent.activeSelf; }
		set {
			if (value != visualizerParent.activeSelf) {
				visualizerParent.SetActive (value);
				if (value) {
					List<Path> tempChoices = immediateChoicesForDog;
					foreach (Path p in allPaths) {
						if (tempChoices.Contains (p)) {
							p.immediateChoiceVisual = true;
						}
						else {
							p.immediateChoiceVisual = false;
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Randomly select the next path the dog should take. Don't call this when you're not going to use it, or things will break.
	/// </summary>
	public List<Tile> SelectNextPath () {
		Path path = immediateChoicesForDog.RandomElement ();
		pathLastTaken = path;
		List<Tile> steps = path.DirectionalPath (m_dog.myTile.stepNode);
		firstPath = null;
		return steps;
	}
}
