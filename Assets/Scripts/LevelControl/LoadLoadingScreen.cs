using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load the loading screen 
/// </summary>
public class LoadLoadingScreen : MonoBehaviour {
	[SerializeField] private int currentLevelNumber;
	private static LoadLoadingScreen staticInstance;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake () {
		staticInstance = this;
	}

	private static AsyncOperation loading;

	/// <summary>
	/// Starts loading the loading screen in the background, but does not switch automatically.
	/// </summary>
	public static void PrimeNextLoadingScreen () {
		staticInstance.StartCoroutine (staticInstance.LoadNewScene (LevelTable.LevelNumberToLoadingSceneIndex (staticInstance.currentLevelNumber + 1)));
	}

	/// <summary>
	/// Starts loading the loading screen in the background, but does not switch automatically.
	/// </summary>
	public static void PrimeRestartLoadingScreen () {
		staticInstance.StartCoroutine (staticInstance.LoadNewScene (LevelTable.LevelNumberToLoadingSceneIndex (staticInstance.currentLevelNumber)));
	}

	/// <summary>
	/// Switch over to the loading screen when ready.
	/// </summary>
	public static void SwitchToLoadingScreen () {
		loading.allowSceneActivation = true;
	}

	private IEnumerator LoadNewScene (int buildIndex) {
		loading = SceneManager.LoadSceneAsync (buildIndex);
		loading.allowSceneActivation = false;
		while (!loading.isDone) {
			yield return null;
		}
	}
}
