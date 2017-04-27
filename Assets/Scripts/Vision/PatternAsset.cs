using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Asset file for a vision pattern.
/// </summary>
public class PatternAsset : ScriptableObject {
	public ProbabilityGrid grid;

	/// <summary>
	/// Create a new pattern asset that can be saved.
	/// </summary>
	public static PatternAsset CreateFromGrid (ProbabilityGrid grid) {
		PatternAsset pattern = ScriptableObject.CreateInstance<PatternAsset> ();
		pattern.grid = grid;
		return pattern;
	}
}
