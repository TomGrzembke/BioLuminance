using UnityEngine;

public class ParticleSound : MonoBehaviour
{
    #region serialized fields
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] AudioSource audioSource;
    [SerializeField] SoundType soundType;
    [SerializeField] PlayType playType;
    [SerializeField] int frequencyDividedBy = 5;
    [SerializeField] bool randomizePitch;
    #endregion

    #region private fields
    int frequencyCounter;
    #endregion

    void Update()
    {
        frequencyCounter++;
        if (frequencyCounter < frequencyDividedBy) return;

        if (randomizePitch)
            audioSource.pitch = Random.Range(0.7f, 2);

        if (playType == PlayType.IsEmmiting)
            if (_particleSystem.isEmitting)
            {
                SoundManager.Instance.PlaySound(soundType, audioSource);
            }

        if (playType == PlayType.ParticleCount)
            if (_particleSystem.particleCount > 0)
            {
                SoundManager.Instance.PlaySound(soundType, audioSource);
            }

        frequencyCounter = 0;
    }

    enum PlayType
    {
        IsEmmiting,
        ParticleCount
    }
}