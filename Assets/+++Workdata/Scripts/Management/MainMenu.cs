using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        GameStateManager.StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
