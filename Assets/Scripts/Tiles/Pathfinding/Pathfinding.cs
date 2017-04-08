using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for finding the shortest path between two floor tiles.
/// </summary>
public static class Pathfinding {
	private class FloorNode {
		public FloorNode (Floor t) {
			myTile = t;
			g = Mathf.Infinity;
			f = Mathf.Infinity;
			cameFrom = null;
		}
		public Floor myTile;
		public float g;
		public float f;
		public FloorNode cameFrom;
	}

	/// <summary>
	/// Finds the best path from start to goal. [0] is the start tile and [Count - 1] is the end tile. Make sure the start and end tiles are included in the availableTiles array.
	/// </summary>
	public static List<Floor> ShortestPath (Floor start, Floor end, List<Floor> availableTiles) {
		FloorNode startNode = null;
		FloorNode endNode = null;

		List<FloorNode> map = new List<FloorNode> ();
		foreach (Floor t in availableTiles) {
			FloorNode tempNode = new FloorNode (t);
			// NOTE THAT THESE ARE SWAPPED SO THAT WE WON'T HAVE TO REVERSE THE ARRAY.
			if (t == start) {
				endNode = tempNode;
			}
			else if (t == end) {
				startNode = tempNode;
			}
			map.Add (tempNode);
		}

		List<FloorNode> closedSet = new List<FloorNode> (); 

		List<FloorNode> openSet = new List<FloorNode> (); 
		openSet.Add (startNode);

		List<FloorNode> scoreMap = new List<FloorNode> ();
		foreach (Floor t in availableTiles) {
			scoreMap.Add (new FloorNode (t));
		}

		startNode.f = HeuristicCostEstimate (startNode, endNode);

		while (openSet.Count > 0) {
			FloorNode current = LowestFScore (openSet);
			if (current == endNode) {
				return ReconstructTilePath (current);
			}
			openSet.Remove (current);
			closedSet.Add (current);

			foreach (FloorNode neighbor in Neighbors(current, map)) {
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
	private static List<Floor> ReconstructTilePath (FloorNode endingNode) {
		List<Floor> totalPath = new List<Floor> ();
		FloorNode current = endingNode;

		while (current != null) {
			totalPath.Add (current.myTile);
			current = current.cameFrom;
		}
		return totalPath;
	}

	/// <summary>
	/// TileNode with the lowest F score in the map.
	/// </summary>
	private static FloorNode LowestFScore (List<FloorNode> test) {
		FloorNode lowest = test [0];
		foreach (FloorNode tn in test) {
			if (tn.f < lowest.f) {
				lowest = tn;
			}
		}
		return lowest;
	}

	/// <summary>
	/// Returns all neighbors in the map of the specified TileNode.
	/// </summary>
	private static List<FloorNode> Neighbors (FloorNode tn, List<FloorNode> map) {
		List<FloorNode> neighbors = new List<FloorNode> ();
		foreach (Compass.Direction dir in Compass.allDirections) {
			FloorNode tmp = GetNodeByTile (tn.myTile.GetNeighborInDirection (dir) as Floor, map);
			if (tmp != null) {
				neighbors.Add (tmp);
			}
		}
		return neighbors;
	}

	/// <summary>
	/// Returns the TileNode in the map corresponding to a particular tile.
	/// </summary>
	private static FloorNode GetNodeByTile (Floor t, List<FloorNode> map) {
		foreach (FloorNode tn in map) {
			if (tn.myTile == t) {
				return tn;
			}
		}
		return null;
	}

	/// <summary>
	/// Distance between two TileNodes.
	/// </summary>
	private static float Distance (FloorNode a, FloorNode b) {
		return Vector3.Distance (a.myTile.topCenterPoint, a.myTile.topCenterPoint);
	}

	/// <summary>
	/// Give some function or what
	/// </summary>
	private static float HeuristicCostEstimate (FloorNode a, FloorNode b) {
		return Distance (a, b);
	}
}
