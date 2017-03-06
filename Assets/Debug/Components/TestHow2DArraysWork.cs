using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHow2DArraysWork : MonoBehaviour {
	void Start () {
		// ... Create 2D array of strings.
		string[,] array = new string[,]
		{
			{"TopLeft", "TopRight"},
			{"MiddleLeft", "MiddleRight"},
			{"BottomLeft", "BottomRight"}
		};

		string overall = "";

		for(int i = 0; i < array.GetLength(0); i++){
			for(int j = 0; j < array.GetLength(1); j++){
				overall += array [i, j];
			}
			overall += "\n";
		}
		print (overall);
		print (array [0, 1]);
		print (array.GetLength (0));
	}
}
