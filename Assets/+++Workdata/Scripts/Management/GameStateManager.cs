using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    static GameStateManager Instance;

    void Awake()
    {
        Instance = this;
    }

    static void StartGame()
    {

    }

    IEnumerator LoadSceneCoroutine(int oldScene, int newScene)
    {
        LoadingScreen.Show(this);
        //yield return SceneLoader.Instance.UnloadSceneViaIndex(newScene);
        yield return SceneLoader.Instance.LoadSceneViaIndex(newScene);
        LoadingScreen.Hide(this);
    }

    static void GoToMainMenu()
    {

    }
}