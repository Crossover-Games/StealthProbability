/// <summary>
/// Cat or dog. If cameras turn out to be implemented significantly differently than dogs, we'll flesh out the machine type.
/// </summary>
public enum CharacterType {
	/// <summary>
	/// Players control cats.
	/// </summary>
	Cat,
	/// <summary>
	/// Dogs are controlled by random AI.
	/// </summary>
	Dog,
	/// <summary>
	/// Doesn't really do much now, but it's the default unimplemented character type for now.
	/// </summary>
	Machine
}
