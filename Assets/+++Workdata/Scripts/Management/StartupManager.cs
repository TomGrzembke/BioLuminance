using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{

    IEnumerator Start()
    {
        //LoadingScreen.Show(this);

        yield return SceneLoader.LoadScene(SceneLoader.DefaultScenes.Manager);
        
                yield return SceneLoader.LoadScene(SceneLoader.DefaultScenes.MainMenu);
        //LoadingScreen.Hide(this);
    }

}