using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// You win if you have cats standing on all of these tiles.
/// </summary>
public class VictoryTile : Floor {
	public override TileType tileType {
		get { return TileType.Objective; }
	}

	private static List<VictoryTile> allVictoryTiles = new List<VictoryTile> ();

	/// <summary>
	/// The minimum number of cats required to finish the mission.
	/// </summary>
	public static int missionCriticalCatCount {
		get { return allVictoryTiles.Count; }
	}

	/// <summary>
	/// True if all victory tiles are occupied by cats.
	/// </summary>
	public static bool gameWon {
		get {
			if (allVictoryTiles.Count == 0) {
				return false;
			}
			foreach (VictoryTile vt in allVictoryTiles) {
				if (vt.occupant == null) {
					return false;
				}
			}
			return true;
		}
	}

	/// <summary>
	/// True if you don't have enough cats to finish the mission.
	/// </summary>
	public static bool gameLost {
		get {
			return GameBrain.catManager.allCharacters.Length < missionCriticalCatCount;
		}
	}

	protected override void LateAwake () {
		allVictoryTiles.Add (this);
	}
}
