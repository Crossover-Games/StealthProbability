using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Something that does something. Ideally, this represents a one-shot action.
/// </summary>
public interface IActionCommand {
	void Execute ();
}
