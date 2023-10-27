using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayLoop : MonoBehaviour
{
    void Awake()
    {
        SceneManager.SetActiveScene(gameObject.scene);
    }

    void Update()
    {
        if(Input.anyKey)
            ReturnToMainMenu();
    }

    public void ReturnToMainMenu()
    {
        LoadingScreen.Show(this);
    }

}