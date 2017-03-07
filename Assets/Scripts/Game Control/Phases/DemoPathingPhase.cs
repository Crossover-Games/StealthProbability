using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPathingPhase : GameControlPhase {
	// override public void OnLeaveControl ()
	// override public void TileClickEvent (Tile t)

	[SerializeField] private PlayerTurnIdlePhase playerTurnIdlePhase;

	[SerializeField] private Dog exampleDog;

	override public void OnTakeControl () {
		foreach (Tile t in brain.tileManager.allTiles) {
			t.dangerColor = DangerSquareVisualizer.RandomColor ();
			t.dangerVisualizerEnabled = true;
		}
	}

	override public void ControlUpdate () {
		exampleDog.DemoMoveAlongTrack ();

		if (Input.GetKey (KeyCode.P)) {
			playerTurnIdlePhase.TakeControl ();
		}
	}

	override public void OnLeaveControl (){
		foreach (Tile t in brain.tileManager.allTiles) {
			t.dangerVisualizerEnabled = false;
		}
	}
}
