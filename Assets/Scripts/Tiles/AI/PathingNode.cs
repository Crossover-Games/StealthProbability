using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An addition to the Tile class representing how dogs move.
/// </summary>
public class PathingNode : MonoBehaviour {

	public string message;

	private Tile tile;
	/// <summary>
	/// The tile this PathingNode is associated with.
	/// </summary>
	public Tile myTile {
		get { return tile; }
	}

	[SerializeField] private bool northConnection;
	[SerializeField] private bool southConnection;
	[SerializeField] private bool eastConnection;
	[SerializeField] private bool westConnection;

	private HashSet<PathingNode> myConnections = new HashSet<PathingNode> ();
	/// <summary>
	/// All neighboring nodes in the guard's path.
	/// </summary>
	public PathingNode[] connections {
		get {
			return myConnections.ToArray ();
		}
	}
		
	[SerializeField] private bool stoppingPoint;
	/// <summary>
	/// If the tile is stopping point, the dog stops on this square for the end of its turn.
	/// </summary>
	public bool isStoppingPoint {
		get { return stoppingPoint; }
	}

	void Awake () {
		tile = GetComponent<Tile> ();
	}
	void Start () {
		Tile northTile = myTile.GetNeighborInDirection (Compass.Direction.North);
		Tile southTile = myTile.GetNeighborInDirection (Compass.Direction.South);
		Tile eastTile = myTile.GetNeighborInDirection (Compass.Direction.East);
		Tile westTile = myTile.GetNeighborInDirection (Compass.Direction.West);

		if (northConnection && northTile != null && northTile.pathingNode != null) {
			myConnections.Add (northTile.pathingNode);
			if (!northTile.pathingNode.southConnection) {
				northTile.pathingNode.southConnection = true;
				northTile.pathingNode.myConnections.Add (this);
			}
		}
		if (southConnection && southTile != null && southTile.pathingNode != null) {
			myConnections.Add (southTile.pathingNode);
			if (!southTile.pathingNode.northConnection) {
				southTile.pathingNode.northConnection = true;
				southTile.pathingNode.myConnections.Add (this);
			}
		}
		if (eastConnection && eastTile != null && eastTile.pathingNode != null) {
			myConnections.Add (eastTile.pathingNode);
			if (!eastTile.pathingNode.westConnection) {
				eastTile.pathingNode.westConnection = true;
				eastTile.pathingNode.myConnections.Add (this);
			}
		}
		if (westConnection && westTile != null && westTile.pathingNode != null) {
			myConnections.Add (westTile.pathingNode);
			if (!westTile.pathingNode.eastConnection) {
				westTile.pathingNode.eastConnection = true;
				westTile.pathingNode.myConnections.Add (this);
			}
		}
	}

	/// <summary>
	/// Given the previous node in a dog's path, what is the next node the dog should take? Null if this node is a stopping point. Returns the previous node for dead ends.
	/// </summary>
	public PathingNode NextOnPath (PathingNode cameFrom) {
		if (stoppingPoint) {
			return null;
		}
		else if (cameFrom == null) {
			//return null;
			return myConnections.RandomElement ();
		}
		else {
			HashSet<PathingNode> remainingNodes = new HashSet<PathingNode> (myConnections);
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

	/// <summary>
	/// From a stopping point, randomly selects the next route to take. No backtracking to the last visited square. Only intended to be called for stopping points; null otherwise.
	/// </summary>
	public PathingNode SelectNextPathStart (Dog dog) {
		if (!stoppingPoint) {
			return null;
		}
		else if (dog.firstTurnNode != null) {
			return dog.firstTurnNode;
		}
		else {
			if (dog.lastVisited == null) {
				return myConnections.RandomElement ();
			}
			else if (dog.firstTurnNode != null) {
				return dog.firstTurnNode;
			}
			else {
				HashSet<PathingNode> remainingNodes = new HashSet<PathingNode> (myConnections);
				remainingNodes.Remove (dog.lastVisited);
				if (remainingNodes.Count == 0) {
					return dog.lastVisited;
				}
				else {
					return remainingNodes.RandomElement ();
				}
			}
		}
	}

	/// <summary>
	/// Returns all potential path starts for a dog who came from a particular node. Only valid for stopping points.
	/// </summary>
	public HashSet<PathingNode> AllPotentialPathStarts (Dog dog) {
		if (!stoppingPoint) {
			return null;
		}
		else if (dog.firstTurnNode != null) {
			HashSet<PathingNode> oneNode = new HashSet<PathingNode> ();
			oneNode.Add (dog.firstTurnNode);
			return oneNode;
		}
		else {
			if (dog.lastVisited == null) {
				return myConnections;
			}
			else {
				HashSet<PathingNode> remainingNodes = new HashSet<PathingNode> (myConnections);
				remainingNodes.Remove (dog.lastVisited);
				if (remainingNodes.Count == 0) {
					HashSet<PathingNode> tmp = new HashSet<PathingNode> ();
					tmp.Add (dog.lastVisited);
					return tmp;
				}
				else {
					return remainingNodes;
				}
			}
		}
	}

	// For visualization in the editor
	void OnDrawGizmos () {
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
		if (northConnection) {
			Gizmos.DrawCube (connectionPoint + Vector3.forward * 0.5f, new Vector3 (0.1f, 0.1f, 1f));
		}
		if (southConnection) {
			Gizmos.DrawCube (connectionPoint + Vector3.back * 0.5f, new Vector3 (0.1f, 0.1f, 1f));
		}
		if (eastConnection) {
			Gizmos.DrawCube (connectionPoint + Vector3.right * 0.5f, new Vector3 (1f, 0.1f, 0.1f));
		}
		if (westConnection) {
			Gizmos.DrawCube (connectionPoint + Vector3.left * 0.5f, new Vector3 (1f, 0.1f, 0.1f));
		}
	}
}
