using MyBox;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    static GameStateManager Instance;

    [Scene, SerializeField] string startScene = "4_Start";

    void Awake() => Instance = this;

    public static void StartGame()
    {
        Instance.StartCoroutine(Instance.LoadScenesCoroutine((int)SceneLoader.DefaultScenes.MainMenu, Instance.GetSceneID(Instance.startScene)));
    }

    public static void GoToMainMenu()
    {
        Instance.StartCoroutine(Instance.LoadScenesCoroutine(SceneManager.GetActiveScene().buildIndex, (int)SceneLoader.DefaultScenes.MainMenu));
    }

    int GetSceneID(string scene)
    {
        return (char)scene[0] - '0';
    }

    IEnumerator LoadScenesCoroutine(int oldScene, int newScene)
    {
        //LoadingScreen.Show(this);
        yield return SceneLoader.Instance.LoadSceneViaIndex(newScene);
        yield return SceneLoader.Instance.UnloadSceneViaIndex(oldScene);
        //LoadingScreen.Hide(this);
    }
}
