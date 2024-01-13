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

    #region Sound
    public void PlaySoundButtonClick()
    {
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
    }
    public void PlaySoundButtonHover()
    {
        SoundManager.Instance.PlaySound(SoundType.ButtonHover);
    }
    public void PlaySoundButtonClickBack()
    {
        SoundManager.Instance.PlaySound(SoundType.ButtonClickBack);
    }
    #endregion
}
