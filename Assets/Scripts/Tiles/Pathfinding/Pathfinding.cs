using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for finding the shortest path between two tiles.
/// </summary>
public static class Pathfinding {
	private class TileNode {
		public TileNode (Tile t) {
			myTile = t;
			g = Mathf.Infinity;
			f = Mathf.Infinity;
			cameFrom = null;
		}
		public Tile myTile;
		public float g;
		public float f;
		public TileNode cameFrom;
	}

	/// <summary>
	/// Finds the best path from start to goal. [0] is the start tile and [Count - 1] is the end tile. Make sure the start and end tiles are included in the availableTiles array.
	/// </summary>
	public static List<Tile> ShortestPath (Tile start, Tile end, List<Tile> availableTiles) {
		TileNode startNode = null;
		TileNode endNode = null;

		List<TileNode> map = new List<TileNode> ();
		foreach (Tile t in availableTiles) {
			TileNode tempNode = new TileNode (t);
			// NOTE THAT THESE ARE SWAPPED SO THAT WE WON'T HAVE TO REVERSE THE ARRAY.
			if (t == start) {
				endNode = tempNode;
			}
			else if (t == end) {
				startNode = tempNode;
			}
			map.Add (tempNode);
		}

		List<TileNode> closedSet = new List<TileNode> (); 

		List<TileNode> openSet = new List<TileNode> (); 
		openSet.Add (startNode);

		List<TileNode> scoreMap = new List<TileNode> ();
		foreach (Tile t in availableTiles) {
			scoreMap.Add (new TileNode (t));
		}

		startNode.f = HeuristicCostEstimate (startNode, endNode);

		while (openSet.Count > 0) {
			TileNode current = LowestFScore (openSet);
			if (current == endNode) {
				return ReconstructTilePath (current);
			}
			openSet.Remove (current);
			closedSet.Add (current);

			foreach (TileNode neighbor in Neighbors(current, map)) {
				if (closedSet.Contains (neighbor)) {
					continue;
				}

				float tentativeGScore = current.g + Distance (current, neighbor);

				if (!openSet.Contains (neighbor)) {
					openSet.Add (neighbor);
				}
				else if (tentativeGScore >= neighbor.g) {
					continue;	// not the best path
				}
					
				// This path is the best until now. Record it!
				neighbor.cameFrom = current;
				neighbor.g = tentativeGScore;
				neighbor.f = neighbor.g + HeuristicCostEstimate (neighbor, endNode);
			}
		}
		return null;
	}

	/// <summary>
	/// Traces back the path and returns a list of all tiles involved.
	/// </summary>
	private static List<Tile> ReconstructTilePath (TileNode endingNode) {
		List<Tile> totalPath = new List<Tile> ();
		TileNode current = endingNode;

		while (current != null) {
			totalPath.Add (current.myTile);
			current = current.cameFrom;
		}
		return totalPath;
	}

	/// <summary>
	/// TileNode with the lowest F score in the map.
	/// </summary>
	private static TileNode LowestFScore (List<TileNode> test) {
		TileNode lowest = test [0];
		foreach (TileNode tn in test) {
			if (tn.f < lowest.f) {
				lowest = tn;
			}
		}
		return lowest;
	}

	/// <summary>
	/// Returns all neighbors in the map of the specified TileNode.
	/// </summary>
	private static List<TileNode> Neighbors (TileNode tn, List<TileNode> map) {
		List<TileNode> neighbors = new List<TileNode> ();
		foreach (Compass.Direction dir in Compass.allDirections) {
			TileNode tmp = GetNodeByTile (tn.myTile.GetNeighborInDirection (dir), map);
			if (tmp != null) {
				neighbors.Add (tmp);
			}
		}
		return neighbors;
	}

	/// <summary>
	/// Returns the TileNode in the map corresponding to a particular tile.
	/// </summary>
	private static TileNode GetNodeByTile (Tile t, List<TileNode> map) {
		foreach (TileNode tn in map) {
			if (tn.myTile == t) {
				return tn;
			}
		}
		return null;
	}

	/// <summary>
	/// Distance between two TileNodes.
	/// </summary>
	private static float Distance (TileNode a, TileNode b) {
		return Vector3.Distance (a.myTile.topCenterPoint, a.myTile.topCenterPoint);
	}

	/// <summary>
	/// Give some function or what
	/// </summary>
	private static float HeuristicCostEstimate (TileNode a, TileNode b) {
		return Distance (a, b);
	}
}
