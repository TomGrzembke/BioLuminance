using MyBox;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    static GameStateManager Instance;

    [Scene, SerializeField] string startScene = "4_Start";
    int startSceneID = 4;

    #region StartSceneSelector

    public int StartSceneID
    {
        get
        {
            startSceneID = (char)Instance.startScene[0] - '0';
            return startSceneID;
        }
    }
    void OnValidate()
    {
        if (Application.isPlaying)
            StartCoroutine(RefreshStartScene());
        else
        {
            Instance = this;
            _ = Instance.StartSceneID;
        }
    }

    IEnumerator RefreshStartScene()
    {
        yield return null;
        _ = Instance.StartSceneID;
    }
    #endregion

    void Awake() => Instance = this;

    public static void StartGame()
    {
        Instance.StartCoroutine(Instance.LoadScenesCoroutine((int)SceneLoader.DefaultScenes.MainMenu, Instance.startSceneID));
    }

    public static void GoToMainMenu()
    {
        Instance.StartCoroutine(Instance.LoadScenesCoroutine(SceneManager.GetActiveScene().buildIndex, (int)SceneLoader.DefaultScenes.MainMenu));
    }

    IEnumerator LoadScenesCoroutine(int oldScene, int newScene)
    {
        //LoadingScreen.Show(this);
        yield return SceneLoader.Instance.UnloadSceneViaIndex(oldScene);
        yield return SceneLoader.Instance.LoadSceneViaIndex(newScene);
        //LoadingScreen.Hide(this);
    }
}
