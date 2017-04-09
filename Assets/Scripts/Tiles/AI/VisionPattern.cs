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
	private float[,] probabilities;

	private int originX;
	private int originY;

	private Dog m_Owner;
	/// <summary>
	/// The dog who owns this vision pattern.
	/// </summary>
	public Dog owner {
		get { return m_Owner; }
	}

	public VisionPattern (Dog theOwner) {
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
			HashSet<Tile> layer1 = new HashSet<Tile> ();
			HashSet<Tile> layer2 = new HashSet<Tile> ();
			foreach (Tile t in m_Owner.myTile.AllTilesInRadius (2, false, false)) {
				if (t.traversable) {
					layer2.Add (t);
				}
			}
			foreach (Tile t in m_Owner.myTile.AllTilesInRadius (1, false, false)) {
				if (t.traversable) {
					layer1.Add (t);
				}
			}
			layer2.ExceptWith (layer1);

			List<TileDangerData> tmp = new List<TileDangerData> ();
			foreach (Tile t in layer1) {
				if (t == m_Owner.myTile.GetNeighborInDirection (m_Owner.orientation)) {
					tmp.Add (new TileDangerData (0.75f, t, m_Owner, Color.red));
				}
				else {
					tmp.Add (new TileDangerData (0.5f, t, m_Owner, Color.yellow));
				}
			}
			foreach (Tile t in layer2) {
				tmp.Add (new TileDangerData (0.25f, t, m_Owner, Color.green));
			}
			tmp.Add (new TileDangerData (1f, m_Owner.myTile, m_Owner, Color.white));
			return tmp;
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
			return probabilities[yIndex, xIndex];
		}
	}
}
