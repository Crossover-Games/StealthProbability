using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A single step on a dog's path.
/// </summary>
public class StepNode : MonoBehaviour {
	private Tile m_tile;
	/// <summary>
	/// The tile this node is associated with.
	/// </summary>
	public Tile myTile {
		get { return m_tile; }
	}

	/// <summary>
	/// Registers tile and finalizes connections.
	/// </summary>
	void Awake () {
		m_tile = GetComponent<Tile> ();
		a_connections = m_connections.ToArray ();
	}

	/// <summary>
	/// Editor only. Do not call in game.
	/// </summary>
	public void SetStoppingPoint (bool state) {
		m_stoppingPoint = state;
	}
	[SerializeField] private bool m_stoppingPoint;
	/// <summary>
	/// Is this node a stopping point?
	/// </summary>
	public bool isStoppingPoint {
		get { return m_stoppingPoint; }
	}


	/// <summary>
	/// Editor only. Do not call in game.
	/// </summary>
	public void RegisterConnection (StepNode sn) {
		if (!m_connections.Contains (sn)) {
			m_connections.Add (sn);
		}
	}

	/// <summary>
	/// Serialized connections. 
	/// </summary>
	[SerializeField] private List<StepNode> m_connections = new List<StepNode> ();
	private StepNode [] a_connections;
	/// <summary>
	/// Tiles immediately connected to this tile.
	/// </summary>
	public StepNode [] connections {
		get { return a_connections; }
	}


	/// <summary>
	/// Editor only. Do not call game.
	/// </summary>
	public void SetSerializedPath (Path p) {
		m_path = p;
	}
	[SerializeField] private Path m_path;
	/// <summary>
	/// Null for stopping points.
	/// </summary>
	public Path myPath {
		get {
			if (isStoppingPoint) {
				return null;
			}
			else {
				return m_path;
			}
		}
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


	void OnDrawGizmos () {
		// this is the top center point of the tile
		Vector3 connectionPoint = new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z);

		if (isStoppingPoint) {
			Gizmos.color = new Color (0.75f, 0f, 0f, 0.75f);
			Gizmos.DrawSphere (connectionPoint, 0.25f);
		}
		else {
			Gizmos.color = new Color (0.25f, 0.25f, 1f, 0.75f);
			Gizmos.DrawSphere (connectionPoint, 0.1f);
		}
		Gizmos.color = new Color (0f, 0f, 0f, 0.75f);
		foreach (StepNode sn in m_connections) {
			Vector3 size = Vector3.one;
			if (Mathf.Abs (transform.position.x - sn.transform.position.x) < 1) {
				size = new Vector3 (0.1f, 0.1f, 1f);
			}
			else {
				size = new Vector3 (1f, 0.1f, 0.1f);
			}
			Gizmos.DrawCube (transform.position.HalfwayTo (sn.transform.position) + Vector3.up * 0.5f, size);
			float distance = Vector3.Distance (transform.position, sn.transform.position);
			if (distance > 1f) {
				print (distance);
			}
		}
	}
}
