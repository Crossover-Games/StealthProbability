using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// A loading screen loads the next level automatically.
/// </summary>
public class MainMenuShortcut : MonoBehaviour {
	private static MainMenuShortcut staticInstance;
	void Awake () {
		staticInstance = this;
	}
	static AsyncOperation async;
	public static void ToMainMenu () {
		if (async == null || async.isDone) {
			staticInstance.StartCoroutine (LoadNewScene (LevelTable.mainMenuBuildIndex));
		}
	}

	static IEnumerator LoadNewScene (int buildIndex) {
		async = SceneManager.LoadSceneAsync (buildIndex);
		while (!async.isDone) {
			yield return null;
		}
	}

}