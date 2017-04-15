using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Succ-sessor to the old pathing node. Not finished.
/// </summary>
public class StepNode : MonoBehaviour {
	private Tile m_tile;
	/// <summary>
	/// The tile this node is associated with.
	/// </summary>
	public Tile myTile {
		get { return null; }
	}

	void Awake () {
		m_tile = GetComponent<Tile> ();
	}

	[SerializeField] private bool m_stoppingPoint;
	/// <summary>
	/// Is this node a stopping point?
	/// </summary>
	public bool isStoppingPoint {
		get { return m_stoppingPoint; }
	}

	/// <summary>
	/// Tiles immediately connected to this tile.
	/// </summary>
	public List<StepNode> connections;

	/// <summary>
	/// Not implemented. Null for stopping points.
	/// </summary>
	public Path myPath {
		get { return null; }
	}

	/// <summary>
	/// Given the previous node in a dog's path, what is the next node the dog should take? Null if this node is a stopping point. Returns the previous node for dead ends.
	/// </summary>
	public StepNode NextOnPath (StepNode cameFrom) {
		if (isStoppingPoint) {
			return null;
		}
		else if (cameFrom == null) {
			return connections.RandomElement ();
		}
		else {
			HashSet<StepNode> remainingNodes = new HashSet<StepNode> (connections);
			remainingNodes.Remove (cameFrom);

			if (remainingNodes.Count == 1) {
				return remainingNodes.ToArray () [0];
			}
			else if (remainingNodes.Count == 0) {
				return cameFrom;
			}
			else {
				return null;
			}
		}
	}
}
