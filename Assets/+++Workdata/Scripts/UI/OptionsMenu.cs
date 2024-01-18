using MyBox;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    #region serialized fields
    [SerializeField] SceneReference ingameScene;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject pauseMenu;

    #endregion

    #region private fields
    Scene[] AllActiveScenes => GetAllScenes();
    bool ingameBool;

    #endregion

    public void CheckContext()
    {
        ingameBool = AllActiveScenes.Contains(SceneManager.GetSceneByName(ingameScene.SceneName));

        optionsMenu.SetActive(!ingameBool);
        pauseMenu.SetActive(ingameBool);
    }

    Scene[] GetAllScenes()
    {
        int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];

        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
        }
        return loadedScenes;
    }

    void OnEnable()
    {
        CheckContext();
    }
}