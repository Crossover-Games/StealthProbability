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

    public void Start() {
        level = Level.Level1;
    }

    public void nexLevel() {
        if(level = Level.Level1) {
            SceneManager.LoadScene("LoadingScreenLevel2Screen1");
        } else if (level = Level.Level2) {
            SceneManager.LoadScene("LoadingScreenLevel3Screen1");
        } else {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
