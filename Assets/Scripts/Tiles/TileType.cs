
public enum TileType {
	Floor,		// anyone can stand on it
	Wall,		// cannot be traversed. does not influence visibility in itself; the actual geometry will do that
	EntryPoint,	// Player can place units here before the game starts. When the game starts, these should turn into normal floors.
	Objective	// probably a placeholder
}
