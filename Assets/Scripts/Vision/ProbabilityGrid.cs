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
}
