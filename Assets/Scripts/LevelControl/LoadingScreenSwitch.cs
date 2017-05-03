using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// A loading screen loads the next level automatically.
/// </summary>
public class LoadingScreenSwitch : MonoBehaviour {
	[SerializeField] private int nextLevelNumber;
	void Start () {
		StartCoroutine (LoadNewScene (LevelTable.LevelNumberToSceneIndex (nextLevelNumber)));
	}

	IEnumerator LoadNewScene (int buildIndex) {
		yield return new WaitForSeconds (3);
		AsyncOperation async = SceneManager.LoadSceneAsync (buildIndex);
		while (!async.isDone) {
			yield return null;
		}
	}

}