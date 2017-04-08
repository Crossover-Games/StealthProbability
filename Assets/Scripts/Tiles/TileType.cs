
public enum TileType {
	Floor,		// anyone can stand on it
	Wall,		// cannot be traversed.
	EntryPoint,	// Player can place units here before the game starts. When the game starts, these should turn into normal floors.
	SinglePointObjective,	// Immediate win when a cat stands on it.
	HoldObjective,			// Win when all tiles of this type are occupied by cats
	OneShotButton,			// Placeholder. Does something when a cat first steps on it
	HoldButton				// Placeholder. Does something while a cat is standing on it.
}
