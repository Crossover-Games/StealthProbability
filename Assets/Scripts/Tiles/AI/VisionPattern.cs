using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionPattern : MonoBehaviour {

	// probabilities[y,x]
	// y: dim0, x: dim1
	// y+ goes down
	// x+ goes right
	// (0,0): top left corner
	private float[,] probabilities;

	private int originX;
	private int originY;

	/// <summary>
	/// Gets the probability of a square a certain number of squares forward/back and right/left of the dog facing a certain direction. 
	/// </summary>
	public float GetProbability (int forward, int right, Compass.Direction direction) {
		/* NORTH
		 *  forward y-, right x+
		 * EAST
		 *  forward y-, right x+
		*/
		int xOffset, yOffset;

		switch (direction) {
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
}
