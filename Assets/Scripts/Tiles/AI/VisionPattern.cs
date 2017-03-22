﻿/// 
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vision pattern. Incomplete.
/// </summary>
public class VisionPattern : MonoBehaviour {

	// probabilities[y,x]
	// y: dim0, x: dim1
	// y+ goes down
	// x+ goes right
	// (0,0): top left corner
	private float[,] probabilities;

	private int originX;
	private int originY;

	[SerializeField] private Dog m_Owner;
	/// <summary>
	/// The dog who owns this vision pattern.
	/// </summary>
	public Dog myOwner {
		get { return m_Owner; }
	}

	/// <summary>
	/// NOT IMPLEMENTED
	/// All floor tiles affected by this vision pattern's sight, and the danger value associated with each.
	/// This will change depending on the orientation and position of the dog.
	/// </summary>
	/// <value>All tiles affected.</value>
	public List<TileDangerPair> allTilesAffected {
		get { return null; }
	}

	/// <summary>
	/// Not mandatory. 
	/// Gets the probability of a square a certain number of squares forward/back and right/left of the dog. Adjusted for dog orientation.
	/// </summary>
	public float GetProbability (int forward, int right) {
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
}
