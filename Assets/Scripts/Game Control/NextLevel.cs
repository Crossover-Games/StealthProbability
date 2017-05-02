using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {

	private AssetBundle loadedAssets;
	private enum Level {
		Level1,
		Level2,
		Level3
	};

	private Level level;

	public void Start () {
		level = Level.Level1;
	}

	public void nexLevel () {
		SceneManager.LoadScene (1);
	}
}
