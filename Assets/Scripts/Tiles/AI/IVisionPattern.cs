using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisionPattern {
	/// <summary>
	/// All floor tiles affected by this vision pattern's sight, and the danger value associated with each.
	/// This will change depending on the orientation and position of the dog.
	/// </summary>
	List<TileDangerData> allTilesAffected{ get; }

}
