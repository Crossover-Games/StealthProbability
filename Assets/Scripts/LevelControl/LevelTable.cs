using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTable {
	/// <summary>
	/// Given a level number in game, return the build index for that scene. 
	/// </summary>
	public static int LevelNumberToSceneIndex (int levelNumber) {
		switch (levelNumber) {
			case 0:
				return 1;
			case 1:
				return 2;
			case 2:
				return 3;
			case 3:
				return 4;
			default:
				return mainMenuBuildIndex;
		}
	}

	/// <summary>
	/// Given a level number in game, return the build index for the corresponding loading screen. 
	/// </summary>
	public static int LevelNumberToLoadingSceneIndex (int levelNumber) {
		switch (levelNumber) {
			case 0:
				return 5;
			case 1:
				return 6;
			case 2:
				return 7;
			case 3:
				return 8;
			default:
				return mainMenuBuildIndex;
		}
	}

	/// <summary>
	/// Build index for main menu screen.
	/// </summary>
	public static int mainMenuBuildIndex {
		get { return 0; }
	}
}
