using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region serialized fields
    public static SoundManager Instance;
    [SerializeField] SoundTypeSO[] soundBank;
    [SerializeField] AudioSource globalMusicSource;
    [SerializeField] AudioSource globalSFXSource;
    #endregion

    #region private fields

    #endregion
    void Awake()
    {
        Instance = this;
    }

    public void PlaySound(SoundType type, AudioSource localSource = null)
    {
        AudioClip clip = null;

        for (int i = 0; i < soundBank.Length; i++)
        {
            if (soundBank[i].soundType != type) continue;

            clip = soundBank[i].clips[Random.Range(0, soundBank[i].clips.Length)];
            break;
        }

        if (localSource && clip != null)
            localSource.PlayOneShot(clip);
        else
            globalSFXSource.PlayOneShot(clip);
    }

}

public enum SoundType
{
    Null,
    ButtonHover,
    ButtonClick,
    ButtonClickConfirm,
    ButtonClickBack,

    Bubble,

    PointCounter,

}