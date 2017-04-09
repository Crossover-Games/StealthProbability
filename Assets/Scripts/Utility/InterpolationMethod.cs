/// <summary>
/// Different methods for interpolation. Use them with Interpolation.Interpolate(...)
/// </summary>
public enum InterpolationMethod {
	/// <summary>
	/// Constant change from t = 0 to t = 1.
	/// </summary>
	Linear,
	/// <summary>
	/// Greatest change with t close to 1.
	/// </summary>
	Quadratic,
	/// <summary>
	/// Greatest change with t close to 0
	/// </summary>
	SquareRoot,
	/// <summary>
	/// Greatest change with t close to 0.5.
	/// </summary>
	Sinusoidal,
	/// <summary>
	/// Greatest change with t close to 0 and 1. Least change with t close to 0.5. 
	/// </summary>
	Cubic

	// Crazy interpolation type: extends past 
}