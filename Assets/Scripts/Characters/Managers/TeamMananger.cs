using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a whole team of characters. This will be used for the cat and dog managers.
/// </summary>
public class TeamMananger<T> where T : GameCharacter {

	public TeamMananger (List<T> characters) {
		m_characters = characters;
	}

	/// <summary>
	/// All characters on this team. Encapsulated by allCharacters.
	/// </summary>
	private List<T> m_characters;

	/// <summary>
	/// All characters on this team.
	/// </summary>
	public T[] allCharacters {
		get { return m_characters.ToArray (); }
	}

	/// <summary>
	/// All characters currently available to move.
	/// </summary>
	public T[] availableCharacters {
		get {
			List<T> tmp = new List<T> ();
			foreach (T character in m_characters) {
				if (!character.grayedOut) {
					tmp.Add (character);
				}
			}
			return tmp.ToArray ();
		}
	}

	/// <summary>
	/// True if at least one character is not grayed out. False if all characters are grayed out.
	/// </summary>
	public bool anyAvailable {
		get {
			foreach (T character in m_characters) {
				if (!character.grayedOut) {
					return true;
				}
			}
			return false;
		}
	}

	/// <summary>
	/// Ungrays all characters and allows them to move again.
	/// </summary>
	public void RejuvenateAll () {
		foreach (T character in m_characters) {
			character.grayedOut = false;
		}
	}

	/// <summary>
	/// Remove the specified character from this team. Used when a cat is detected and removed from the board.
	/// </summary>
	public void Remove (T character) {
		if (character != null && m_characters.Contains (character)) {
			m_characters.Remove (character);
			character.myTile.SetOccupant (null);
		}
	}

	/// <summary>
	/// Randomly reorder the indices of the team members.
	/// </summary>
	public void Shuffle () {
		m_characters.Shuffle ();
	}
}
