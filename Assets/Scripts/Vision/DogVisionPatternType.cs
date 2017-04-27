using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All different types of dog vision patterns.
/// </summary>
public enum DogVisionPatternType {
	/// <summary>
	/// When all else fails, produce some stock pattern.
	/// </summary>
	Default,
	/// <summary>
	/// Grunts introduced early on.
	/// </summary>
	Hound,
	/// <summary>
	/// FAKE. Impractical monster with a deadly beam of vision straight ahead.
	/// </summary>
	Cyclops,
	/// <summary>
	/// Chihuahua. Very small. Truncated to prevent spelling errors.
	/// </summary>
	Chih
}
