using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serializable rectangular array of probability floats.
/// </summary>
[System.Serializable]
public class ProbabilityGrid : FlatArray2D<float> {
	/// <summary>
	/// Create a new square grid.
	/// </summary>
	public static ProbabilityGrid CreateEmptyGrid (int size) {
		ProbabilityGrid pg = new ProbabilityGrid ();
		pg.Set2DShallow (new float [size, size]);
		return pg;
	}


	/// <summary>
	/// Loads a ProbabilityGrid from Resources/VisionPatterns 
	/// </summary>
	/// <param name="size"></param>
	/// <returns></returns>
	public static ProbabilityGrid LoadFromResources (string fileName) {
		return JsonUtility.FromJson<ProbabilityGrid> (Resources.Load<TextAsset> ("VisionPatterns/" + fileName).text);
	}
}
