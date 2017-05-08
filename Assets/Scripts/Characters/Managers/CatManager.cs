using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : TeamMananger<Cat> {

	public CatManager (List<Cat> characters) : base (characters) { }

	/// <summary>
	/// Remove the specified character from this team. Used when a cat is detected and removed from the board.
	/// </summary>
	public override void Remove (Cat character) {
		base.Remove (character);
		DetectionManager.ClearDangerByCat (character);
	}

	public override void Rejuvenate (Cat character) {
		base.Rejuvenate (character);
		character.stealthStacks = 0;
	}
}
