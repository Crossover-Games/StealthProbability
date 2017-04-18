using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogNames : MonoBehaviour {
	[TextArea (0, 300)]
	public string dogJson;
	private string [] dogWords;

	[System.Serializable]
	private class WordHolder {
		public string [] vocabulary;
	}

	void Awake () {
		dogWords = JsonUtility.FromJson<WordHolder> (dogJson).vocabulary;
		foreach (Dog d in FindObjectsOfType<Dog> ()) {
			d.name = CreateDogName ();
			d.route.name = d.name + "'s Route";
		}
	}

	private string CreateDogName () {
		int index1 = UnityEngine.Random.Range (0, dogWords.Length);
		int index2 = index1;
		while (index1 == index2) {
			index2 = UnityEngine.Random.Range (0, dogWords.Length);
		}
		return dogWords [index1] + " " + dogWords [index2];
	}

}
