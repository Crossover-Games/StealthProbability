using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelBuilderRemake {
	/// <summary>
	/// Builds a level from a level blueprint.
	/// </summary>
	public class LevelAssembler : ScriptableObject {
		public GameObject floorTilePrefab;
		public GameObject wallTilePrefab;
		public GameObject gameControllerPrefab;
		public GameObject dogPrefab;
		public GameObject pathPrefab;
		public GameObject routePrefab;

		public static void AssembleLevel (LevelBlueprint blueprint) {
			LevelAssembler assembler = ScriptableObject.CreateInstance<LevelAssembler> ();

			GameObject found = GameObject.FindGameObjectWithTag ("GameController");
			if (found == null) {
				GameObject g = PrefabUtility.InstantiatePrefab (assembler.gameControllerPrefab) as GameObject;
				g.name = "GameController";
			}

			found = GameObject.FindGameObjectWithTag ("MapTilesParent");
			if (found != null) {
				DestroyImmediate (found);
			}
			for (int i = 0; i < blueprint.tiles.GetLength (0); i++) {
				for (int j = 0; j < blueprint.tiles.GetLength (1); j++) {
					Point2D thisPoint = new Point2D (i, j);
					if (blueprint.victoryTiles.Contains (thisPoint)) {
						//
					}
					else {
						//
					}
				}
			}
		}

		private void InstantiateFloor (Vector3 pos, Transform mapTilesParent) {
			GameObject g = PrefabUtility.InstantiatePrefab (floorTilePrefab) as GameObject;
			g.transform.position = pos;
			g.transform.SetParent (mapTilesParent);
		}
		private void InstantiateWall (Vector3 pos, Transform mapTilesParent) {
			GameObject g = PrefabUtility.InstantiatePrefab (wallTilePrefab) as GameObject;
			g.transform.position = pos;
			g.transform.SetParent (mapTilesParent);
		}
	}
}