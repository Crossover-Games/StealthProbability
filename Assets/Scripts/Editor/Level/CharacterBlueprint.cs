﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelBuilderRemake {
	/// <summary>
	/// All fields that go into instantiating a character on the board.
	/// </summary>
	public abstract class CharacterBlueprint : ScriptableObject {

		/// <summary>
		/// This character's in game name.
		/// </summary>
		public string characterName { get; set; }

		/// <summary>
		/// Starting orientation of this character.
		/// </summary>
		public Compass.Direction orientation { get; set; }

		/// <summary>
		/// Starting position of this character on the board.
		/// </summary>
		public Point2D location { get; set; }

		/// <summary>
		/// Prefab that is instantiated for the character.
		/// </summary>
		[SerializeField] private GameObject characterPrefab;

		/// <summary>
		/// Instantiates the character at its board location with its given name in its starting orientation. Returns the character component.
		/// </summary>
		public GameCharacter InstantiateSelf (Transform parent) {
			GameObject go = (PrefabUtility.InstantiatePrefab (characterPrefab) as GameObject);
			go.name = characterName;
			go.transform.position = location.ToVector3XZ (0.5f);
			go.transform.rotation = Compass.DirectionToRotation (orientation);
			go.transform.SetParent (parent, true);
			GameCharacter gc = go.GetComponent<GameCharacter> ();
			gc.orientation = orientation;
			return gc;
		}
	}
}