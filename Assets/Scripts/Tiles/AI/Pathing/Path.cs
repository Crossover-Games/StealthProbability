using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A single path a dog can take in one turn.
/// This is a MonoBehaviour so that it can be serialized before the scene runs
/// </summary>
public class Path : MonoBehaviour {

	[SerializeField] private Material immediateChoiceMaterial;
	[SerializeField] private Material futureChoiceMaterial;
	[SerializeField] private GameObject arrowPrefab;

	/// <summary>
	/// Editor only.
	/// </summary>
	public void SetSerializedRoute (Route r) {
		m_route = r;
	}
	[SerializeField] private Route m_route;
	/// <summary>
	/// The route this path is a part of.
	/// </summary>
	public Route myRoute {
		get { return m_route; }
	}

	private List<Renderer> allRenderers = new List<Renderer> ();

	void Start () {
		List<Tile> steps = InitialPath ();
		for (int x = 0; x < steps.Count - 1; x++) {
			GameObject freshArrow = GameObject.Instantiate (arrowPrefab, steps [x].topCenterPoint.HalfwayTo (steps [x + 1].topCenterPoint), Quaternion.identity);
			if (Mathf.Abs (steps [x].transform.position.x - steps [x + 1].transform.position.x) > 0.01f) {
				freshArrow.transform.rotation = Quaternion.Euler (0f, 90f, 0f);
			}
			freshArrow.transform.SetParent (myRoute.visualizerParent.transform);
			allRenderers.Add (freshArrow.GetComponentInChildren<Renderer> ());
		}
	}

	/// <summary>
	/// Creates the path given the two endpoints and the immediate next tile to each.
	/// </summary>
	public void DefinePath (StepNode endA, StepNode nextA, StepNode endB, StepNode nextB) {
		endpointA = endA;
		immediateA = nextA;
		endpointB = endB;
		immediateB = nextB;
	}

	// Serialized so that they are remembered and can be constructed on awake
	[SerializeField] private StepNode endpointA;
	[SerializeField] private StepNode immediateA;
	[SerializeField] private StepNode endpointB;
	[SerializeField] private StepNode immediateB;
	/// <summary>
	/// Given a starting point, return the list of steps it will take to get to the other end of the path. Null if invalid starting point.
	/// </summary>
	public List<Tile> DirectionalPath (StepNode startingPoint) {
		StepNode previous = endpointA;
		StepNode current = immediateA;
		if (startingPoint == endpointA) {
			previous = endpointA;
			current = immediateA;
		}
		else if (startingPoint == endpointB) {
			previous = endpointB;
			current = immediateB;
		}
		else {
			return null;
		}
		List<Tile> steps = new List<Tile> ();
		while (current != null) {
			steps.Add (current.myTile);
			StepNode tempPrevious = current;
			current = current.NextOnPath (previous);
			previous = tempPrevious;
		}
		return steps;
	}

	/// <summary>
	/// Uses an unstable NextOnPath to instantiate the arrow segments.
	/// </summary>
	private List<Tile> InitialPath () {
		StepNode previous = endpointA;
		StepNode current = immediateA;
		List<Tile> steps = new List<Tile> ();
		steps.Add (endpointA.myTile);
		while (current != null) {
			steps.Add (current.myTile);
			StepNode tempPrevious = current;
			current = current.NextOnPath (previous);
			previous = tempPrevious;
		}
		return steps;
	}

	private bool immediate = false;
	/// <summary>
	/// Visual state of this path. True if the dog has this as an immediate option. False if it is a future option.
	/// </summary>
	public bool immediateChoiceVisual {
		get { return immediate; }
		set {
			if (immediate != value) {
				immediate = value;
				foreach (Renderer r in allRenderers) {
					r.material = immediate ? immediateChoiceMaterial : futureChoiceMaterial;
				}
			}
		}
	}
}
