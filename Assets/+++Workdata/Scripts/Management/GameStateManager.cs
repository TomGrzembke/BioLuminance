using MyBox;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    static GameStateManager Instance;

    [Scene, SerializeField] string startScene;
    [SerializeField] GameObject optionsWindow;

    void Awake() => Instance = this;

    public static void StartGame()
    {
        Instance.StartCoroutine(Instance.LoadScenesCoroutine((int)Scenes.MainMenu, Instance.GetSceneID(Instance.startScene)));
    }

    public static void OptionsWindow(bool status = true)
    {
        Instance.optionsWindow?.SetActive(status);
    }

    public static void GoToMainMenu()
    {
        Instance.StartCoroutine(Instance.LoadScenesCoroutine(SceneManager.GetActiveScene().buildIndex, (int)Scenes.MainMenu));
    }

    /// <summary> Depends on the naming (0_Scene), since its gets the first char and ints it</summary>
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