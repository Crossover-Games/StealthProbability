/// 
/// WHAT WE NEED
/// 
/// * List<TileDangerPair> allTilesAffected { get; }
/// All floor tiles affected by this vision pattern's sight, and the danger value associated with each.
/// This will change depending on the orientation and position of the dog.
/// We need this for detection checks.
/// 
/// * A way to load and store vision patterns as a resource. 
/// Since multiple dogs will have the same vision pattern, but there will still multiple kinds of vision patterns, 
/// this is important. This is especially so because this is probably going to be one of the first things we tweak for balance.
/// A text file is perfectly fine.
/// 
/// Everything that's here so far is untested and incomplete. You're welcome to use it as a start if it's helpful, but it
/// very well may not be. Feel free to scrap anything too, or just start over if you like.
/// 
/// 

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

	public VisionPattern (Dog theOwner, string patternFile) {
		if (!patternFile.Equals ("FAKE")) {
			using (StreamReader sw = new StreamReader (patternFile)) {
				string patternJSON = sw.ReadToEnd ();
				Pattern pattern = JsonUtility.FromJson<Pattern> (patternJSON);
				this.probabilities = pattern.probabilities;
			}
      } else {
        this.probabilities = new float[,] {{0f, 0.5f, 0f},
                                           {0.25f, 1f, 0.25f},
                                           {0f, 0.25f, 0f}};
    }
		m_Owner = theOwner;
		}

	/// <summary>
	/// NOT IMPLEMENTED CURRENTLY FAKING
	/// All floor tiles affected by this vision pattern's sight, and the danger value associated with each.
	/// This will change depending on the orientation and position of the dog.
	/// </summary>
	/// <value>All tiles affected.</value>
	public List<TileDangerData> allTilesAffected {
		get {
        int radius = (probabilities.GetLength(0) - 1) / 2;
        Tile[,] tiles = adjustTileMatrix(getTilesInRadius(radius));
        List<TileDangerData> dangerList = new List<TileDangerData> ();
        for(int xIdx = 0; xIdx < radius*2 - 1; xIdx++) {
            for(int yIdx = 0; yIdx < radius*2 - 1; yIdx++) {
                Color color;
                float probability = probabilities[xIdx, yIdx];
                if(probability - 0.25 < 0.01) {
                    color = Color.green; 
                } else if(probability - 0.5 < 0.01){
                    color = Color.yellow;
                } else if(probability - 0.75 < 0.01){
                    color = Color.red;
                } else if(probability - 1 < 0.01){
                    color = Color.black;
                } else {
                    color = Color.blue;
                }
                dangerList.Add(new TileDangerData(probabilities[xIdx, yIdx], tiles[xIdx, yIdx], m_Owner, color));
            }
        }
        return dangerList;
		}
	}

	/// <summary>
	/// Gets the probability of a square a certain number of squares forward/back and right/left of the dog. Adjusted for dog orientation.
	/// </summary>
	private float GetProbability (int forward, int right) {
		/* NORTH
		 *  forward y-, right x+
		 * EAST
		 *  forward y-, right x+
		*/
		int xOffset, yOffset;

		switch (m_Owner.orientation) {
			case Compass.Direction.North:
				xOffset = right;
				yOffset = -forward;
				break;
			case Compass.Direction.South:
				xOffset = -right;
				yOffset = forward;
				break;
			case Compass.Direction.East:
				xOffset = forward;
				yOffset = right;
				break;
			case Compass.Direction.West:
				xOffset = -forward;
				yOffset = -right;
				break;
			default:
				xOffset = 99;
				yOffset = 99;
				break;
		}

		int xIndex = originX + xOffset;
		int yIndex = originY + yOffset;

		//if outside the array, no danger
		if (xIndex >= probabilities.GetLength (1) || xIndex < 0 || yIndex >= probabilities.GetLength (0) || yIndex < 0) {
			return 0f;
		}
		else {
			return probabilities [yIndex, xIndex];
		}
	}

  /// <summary>
  /// Get a matrix of surronding tiles.
  /// </summary>
  private Tile[,] getTilesInRadius(int radius) {
      Tile[,] tileMatrix = new Tile[radius*2 + 1, radius*2 + 1];
      Queue<Tile> tileQueue = new Queue<Tile>();
      Queue<matIdx> idxQueue = new Queue<matIdx>();

      tileQueue.Enqueue(m_Owner.myTile);
      idxQueue.Enqueue(new matIdx(radius + 1, radius +1));

      while(tileQueue.Count > 0) {
          Tile currentTile = tileQueue.Dequeue();
          matIdx currentIdx = idxQueue.Dequeue();

          tileMatrix[currentIdx.x, currentIdx.y] = currentTile;
          if(currentIdx.y > 0 && tileMatrix[currentIdx.x, currentIdx.y - 1] == null) {
              Tile northTile = currentTile.GetNeighborInDirection(Compass.Direction.North);
              matIdx northIdx = new matIdx(currentIdx.x, currentIdx.y - 1);
              tileQueue.Enqueue(northTile);
              idxQueue.Enqueue(northIdx);
          }
          if(currentIdx.y < radius*2 - 1 && tileMatrix[currentIdx.x, currentIdx.y + 1] == null) {
              Tile southTile = currentTile.GetNeighborInDirection(Compass.Direction.South);
              matIdx southIdx = new matIdx(currentIdx.x, currentIdx.y + 1);
              tileQueue.Enqueue(southTile);
              idxQueue.Enqueue(southIdx);
          }
          if(currentIdx.x > 0 && tileMatrix[currentIdx.x - 1, currentIdx.y] == null) {
              Tile westTile = currentTile.GetNeighborInDirection(Compass.Direction.West);
              matIdx westIdx = new matIdx(currentIdx.x - 1, currentIdx.y);
              tileQueue.Enqueue(westTile);
              idxQueue.Enqueue(westIdx);
          }
          if(currentIdx.x < radius*2 - 1 && tileMatrix[currentIdx.x + 1, currentIdx.y] == null) {
              Tile eastTile = currentTile.GetNeighborInDirection(Compass.Direction.East);
              matIdx eastIdx = new matIdx(currentIdx.x + 1, currentIdx.y);
              tileQueue.Enqueue(eastTile);
              idxQueue.Enqueue(eastIdx);
          }

      }
      return tileMatrix;
  }

  private Tile[,] adjustTileMatrix(Tile[,] matrix) {
      int xLen = matrix.GetLength(0);
      int yLen = matrix.GetLength(1);
      Tile[,] adjustedMatrix = new Tile[xLen, yLen];
      if(m_Owner.orientation == Compass.Direction.South){
          for(int xIdx = 0; xIdx < xLen; xIdx++){
              for(int yIdx = 0; yIdx < yLen; yIdx++) {
                  adjustedMatrix[xIdx, yIdx] = matrix[xLen - 1 - xIdx, yLen - 1 - yIdx];
              }
          }
      } else if(m_Owner.orientation == Compass.Direction.East) {
          for(int xIdx = 0; xIdx < xLen; xIdx++){
              for(int yIdx = 0; yIdx < yLen; yIdx++) {
                  adjustedMatrix[xIdx, yIdx] = matrix[yIdx, xLen - 1 - xIdx];
              }
          }
      } else if(m_Owner.orientation == Compass.Direction.West) {
          for(int xIdx = 0; xIdx < xLen; xIdx++){
              for(int yIdx = 0; yIdx < yLen; yIdx++) {
                  adjustedMatrix[xIdx, yIdx] = matrix[yLen - 1 - yIdx, xIdx];
              }
          }
      } else {
          adjustedMatrix = matrix;
      }
      return adjustedMatrix;
  }
}

struct matIdx {
    public int x, y;

    public matIdx(int p1, int p2) {
        x = p1;
        y = p2;
    }
}
