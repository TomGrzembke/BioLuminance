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
    const string musicVolumeHash = "musicVolume";
    const string sfxVolumeHash = "sfxVolume";
    const string fullscreenHash = "fullscreenID";
    #endregion

    void Start()
    {
        SetMusicVolume(PlayerPrefs.GetFloat(musicVolumeHash));
        SetSfxVolume(PlayerPrefs.GetFloat(sfxVolumeHash));

        bool fullScreen = PlayerPrefs.GetInt(fullscreenHash) == 0;

        Screen.fullScreen = fullScreen;
        screenToggle.isOn = fullScreen;
    }

    public void OnMusicSliderChanged()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat(musicVolumeHash, volume);
        PlayerPrefs.SetFloat(musicVolumeHash, volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicSlider.value = volume;
    }

    public void OnSfxSliderChanged()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat(sfxVolumeHash, volume);
        PlayerPrefs.SetFloat(sfxVolumeHash, volume);
    }

    public void SetSfxVolume(float volume)
    {
        sfxSlider.value = volume;
    }

    public void FullScreenToggle(bool isFullscreen)
    {
        isFullscreen = screenToggle.isOn;
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(fullscreenHash, (isFullscreen ? 0 : 1));
    }

    public void OpenURL(string link)
    {
        Application.OpenURL(link);
    }
}
