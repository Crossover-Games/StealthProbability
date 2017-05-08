/*
 - Must be square.
 - Origin is center.
 - Length of each side must be an odd number.
 */

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vision pattern. Incomplete.
/// </summary>
public class VisionPattern {

	// probabilities[y,x]
	// y: dim0, x: dim1
	// y+ goes down
	// x+ goes right
	// (0,0): top left corner
	private float [,] probabilities;

	private int originX;
	private int originY;

	private Dog m_Owner;
	/// <summary>
	/// The dog who owns this vision pattern.
	/// </summary>
	public Dog owner {
		get { return m_Owner; }
	}

	/// <summary>
	/// Only for lasers.
	/// </summary>
	public VisionPattern (Dog theOwner) {
		m_Owner = theOwner;
	}

	/// <summary>
	/// Creates a vision pattern from a ProbabilityGrid in Resources/VisionPatterns. FileName has no extension and path.
	/// </summary>
	public VisionPattern (Dog theOwner, string fileName) {
		if (!fileName.Equals ("FAKE")) {
			this.probabilities = ProbabilityGrid.LoadFromResources (fileName).Get2DShallow ();
		}
		else {
			this.probabilities = new float [,] {{0.75f, 0f, 0f, 0f, 0.75f},
										   {0f, 0.5f, 0f, 0.5f, 0f},
										   {0f, 0f, 0.25f, 0f, 0f},
										   {0f, 0.5f, 0f, 0.5f, 0f},
										   {0.75f, 0f, 0f, 0f, 0.75f}};
		}
		if (probabilities.GetLength (0) != probabilities.GetLength (1)) {
			throw new System.ArgumentException ("Vision Pattern isn't square. See Pattern.cs.");
		}
		else if (probabilities.GetLength (0) % 2 == 0) {
			throw new System.ArgumentException ("Vision Pattern has even side. See Pattern.cs");
		}
		m_Owner = theOwner;
	}

	/// <summary>
	/// All floor tiles affected by this vision pattern's sight, and the danger value associated with each.
	/// This will change depending on the orientation and position of the dog.
	/// </summary>
	public virtual List<TileDangerData> allTilesAffected {
		get {
			int radius = (probabilities.GetLength (0) - 1) / 2;
			Tile [,] tiles = adjustTileMatrix (getTilesInRadius (radius));
			//Tile [,] tiles = getTilesInRadius (radius);
			Stack<TileDangerData> dangerStack = new Stack<TileDangerData> ();
			for (int xIdx = 0; xIdx < probabilities.GetLength (0); xIdx++) {
				for (int yIdx = 0; yIdx < probabilities.GetLength (1); yIdx++) {
					float probability = probabilities [yIdx, xIdx];
					if (tiles [yIdx, xIdx] != null && probability > 0.01f) {
						dangerStack.Push (new TileDangerData (probability, tiles [yIdx, xIdx], m_Owner));
					}
				}
			}
			return dangerStack.ToList ();
		}
	}

	/// <summary>
	/// Get a matrix of surrounding tiles.
	/// </summary>
	private Tile [,] getTilesInRadius (int radius) {
		Tile [,] tileMatrix = new Tile [radius * 2 + 1, radius * 2 + 1];
		Queue<Tile> tileQueue = new Queue<Tile> ();
		Queue<matIdx> idxQueue = new Queue<matIdx> ();

		tileQueue.Enqueue (m_Owner.myTile);
		idxQueue.Enqueue (new matIdx (radius, radius));

		int count = 0;
		while (tileQueue.Count > 0) {
			count++;
			Tile currentTile = tileQueue.Dequeue ();
			matIdx currentIdx = idxQueue.Dequeue ();

			tileMatrix [currentIdx.y, currentIdx.x] = currentTile;
			if (currentIdx.y > 0 && tileMatrix [currentIdx.y - 1, currentIdx.x] == null) {
				Tile northTile = currentTile.GetNeighborInDirection (Compass.Direction.North);
				matIdx northIdx = new matIdx (currentIdx.x, currentIdx.y - 1);
				if (northTile != null) {
					tileQueue.Enqueue (northTile);
					idxQueue.Enqueue (northIdx);
				}
			}
			if (currentIdx.y < radius * 2 && tileMatrix [currentIdx.y + 1, currentIdx.x] == null) {
				Tile southTile = currentTile.GetNeighborInDirection (Compass.Direction.South);
				matIdx southIdx = new matIdx (currentIdx.x, currentIdx.y + 1);
				if (southTile != null) {
					tileQueue.Enqueue (southTile);
					idxQueue.Enqueue (southIdx);
				}
			}
			if (currentIdx.x > 0 && tileMatrix [currentIdx.y, currentIdx.x - 1] == null) {
				Tile westTile = currentTile.GetNeighborInDirection (Compass.Direction.West);
				matIdx westIdx = new matIdx (currentIdx.x - 1, currentIdx.y);
				if (westTile != null) {
					tileQueue.Enqueue (westTile);
					idxQueue.Enqueue (westIdx);
				}
			}
			if (currentIdx.x < radius * 2 && tileMatrix [currentIdx.y, currentIdx.x + 1] == null) {
				Tile eastTile = currentTile.GetNeighborInDirection (Compass.Direction.East);
				matIdx eastIdx = new matIdx (currentIdx.x + 1, currentIdx.y);
				if (eastTile != null) {
					tileQueue.Enqueue (eastTile);
					idxQueue.Enqueue (eastIdx);
				}
			}
		}
		return tileMatrix;
	}

	private Tile [,] adjustTileMatrix (Tile [,] matrix) {
		int xLen = matrix.GetLength (0);
		int yLen = matrix.GetLength (1);
		Tile [,] adjustedMatrix = new Tile [xLen, yLen];
		if (m_Owner.orientation == Compass.Direction.South) {
			for (int xIdx = 0; xIdx < xLen; xIdx++) {
				for (int yIdx = 0; yIdx < yLen; yIdx++) {
					adjustedMatrix [xIdx, yIdx] = matrix [xLen - 1 - xIdx, yLen - 1 - yIdx];
				}
			}
		}
		else if (m_Owner.orientation == Compass.Direction.East) {
			for (int xIdx = 0; xIdx < xLen; xIdx++) {
				for (int yIdx = 0; yIdx < yLen; yIdx++) {
					adjustedMatrix [xIdx, yIdx] = matrix [yIdx, xLen - 1 - xIdx];
				}
			}
		}
		else if (m_Owner.orientation == Compass.Direction.West) {
			for (int xIdx = 0; xIdx < xLen; xIdx++) {
				for (int yIdx = 0; yIdx < yLen; yIdx++) {
					adjustedMatrix [xIdx, yIdx] = matrix [yLen - 1 - yIdx, xIdx];
				}
			}
		}
		else {
			adjustedMatrix = matrix;
		}
		return adjustedMatrix;
	}

	public static VisionPattern VisionPatternFromType (Dog theOwner, PatternType type) {
		try {
			return new VisionPattern (theOwner, type.ToString ());
		}
		catch {
			return new VisionPattern (theOwner, "FAKE");
		}
	}

	/// <summary>
	/// All different types of dog vision patterns.
	/// </summary>
	public enum PatternType {
		/// <summary>
		/// Grunts introduced early on.
		/// </summary>
		Hound,
		/// <summary>
		/// Chihuahua. Very small. Truncated to prevent spelling errors.
		/// </summary>
		Chih,
		Pug,
		TrainingBot,
		Beacon
	}

	/// <summary>
	/// Returns a description of each type of dog by vision pattern.
	/// </summary>
	public static string PatternTypeDescription (PatternType type) {
		switch (type) {
			case PatternType.Hound:
				return "Hound type dog: Sharper senses than the average dog";
			case PatternType.Chih:
				return "Chihuahua type dog: Has more bark than bite, except in large numbers";
			case PatternType.Beacon:
				return "Beacon: Immobile anti-cat security equipment";
			case PatternType.Pug:
				return "Pug: Adorable pug eyes make for better lateral vision at the cost of having poor forward vision.";
			case PatternType.TrainingBot:
				return "Training bot dog: A weak mockery of our enemy used for instructional purposes.";
			default:
				return "Unique: An unpredictable unit in canine security";
		}
	}
}

struct matIdx {
	public int x, y;

	public matIdx (int p1, int p2) {
		x = p1;
		y = p2;
	}
}
