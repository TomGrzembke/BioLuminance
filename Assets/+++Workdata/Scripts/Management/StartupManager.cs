using System.Collections;
using UnityEngine;

public class StartupManager : MonoBehaviour
{
    IEnumerator Start()
    {
        //LoadingScreen.Show(this);
        yield return SceneLoader.LoadScene(SceneLoader.DefaultScenes.Manager);
        yield return SceneLoader.LoadScene(SceneLoader.DefaultScenes.MainMenu);
        yield return SceneLoader.LoadScene(SceneLoader.DefaultScenes.IngameUI);
        //LoadingScreen.Hide(this);
    } 
}