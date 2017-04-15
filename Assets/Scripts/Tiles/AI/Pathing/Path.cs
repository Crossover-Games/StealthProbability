using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NOT IMPLEMENTED
/// A single route a dog can take in one turn.
/// </summary>
public class Path {
	private StepNode endpointA;
	private StepNode immediateA;
	private StepNode endpointB;
	private StepNode immediateB;
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
	/// Visual state of this path. True if the dog has this as an immediate option. False if it is a future option.
	/// </summary>
	public bool immediateChoiceVisual {
		get { return true; }
		set { }
	}
}
