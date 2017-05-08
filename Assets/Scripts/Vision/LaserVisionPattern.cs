using System.Collections.Generic;

public class LaserVisionPattern : VisionPattern {
	public LaserVisionPattern (Laser theOwner) : base (theOwner) {
		//
	}
	public override List<TileDangerData> allTilesAffected {
		get {
			Stack<TileDangerData> dangerStack = new Stack<TileDangerData> ();
			dangerStack.Push (new TileDangerData (1f, owner.myTile, owner));
			Tile currentTile = owner.myTile.GetNeighborInDirection (owner.orientation);
			while (Tile.ValidStepDestination (currentTile)) {
				dangerStack.Push (new TileDangerData ((owner as Laser).probability, currentTile, owner));
				currentTile = currentTile.GetNeighborInDirection (owner.orientation);
			}
			return dangerStack.ToList ();
		}
	}
}
