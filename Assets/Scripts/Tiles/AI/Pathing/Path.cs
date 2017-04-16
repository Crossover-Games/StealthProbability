using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A single route a dog can take in one turn.
/// This is a MonoBehaviour so that it can be serialized before the scene runs
/// </summary>
public class Path : MonoBehaviour {

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
	/// Not implemented.
	/// Visual state of this path. True if the dog has this as an immediate option. False if it is a future option.
	/// </summary>
	public bool immediateChoiceVisual {
		get { return true; }
		set { }
	}
}
