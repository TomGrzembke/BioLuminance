using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        GameStateManager.StartGame();
    }
    public void OptionsWindow()
    {
       GameStateManager.OptionsWindow();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
