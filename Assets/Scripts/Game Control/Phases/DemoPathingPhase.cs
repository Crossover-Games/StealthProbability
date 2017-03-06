using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPathingPhase : GameControlPhase {
	// override public void OnLeaveControl ()
	// override public void TileClickEvent (Tile t)

	[SerializeField] private PlayerTurnIdlePhase playerTurnIdlePhase;

	private const float timeToNextSquare = 0.5f;
	private float elapsedTime = 0f;

	[SerializeField] private Dog exampleDog;

	override public void OnTakeControl () {
		elapsedTime = 0f;
		foreach (Tile t in brain.tileManager.allTiles) {
			t.dangerColor = DangerSquareVisualizer.RandomColor ();
			t.dangerVisualizerEnabled = true;
		}
	}

	override public void ControlUpdate () {
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= timeToNextSquare) {
			elapsedTime = 0f;
			exampleDog.DemoMoveAlongTrack ();
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			playerTurnIdlePhase.TakeControl ();
		}
	}

	override public void OnLeaveControl (){
		foreach (Tile t in brain.tileManager.allTiles) {
			t.dangerVisualizerEnabled = false;
		}
	}
}
