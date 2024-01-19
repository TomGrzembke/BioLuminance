using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    #region Serilized Fields
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider, sfxSlider;
    [SerializeField] Toggle screenToggle;
    #endregion

    #region private
    #endregion

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        OnMusicSliderChanged();

        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        OnSfxSliderChanged();

        GetScreenToggle();
    }

    public void OnMusicSliderChanged()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("musicVolume", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void OnSfxSliderChanged()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("sfxVolume", volume);
        PlayerPrefs.SetFloat("sfxVolume", volume);
        sfxSlider.value = volume;
    }

    void GetScreenToggle()
    {
        bool isFullScreen = PlayerPrefs.GetInt("fullscreenID") == 0; ;
        screenToggle.isOn = isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    public void FullScreenToggle()
    {
        bool isFullscreen = screenToggle.isOn;
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("fullscreenID", (isFullscreen ? 0 : 1));
    }

    public void OpenURL(string link)
    {
        Application.OpenURL(link);
    }
}
