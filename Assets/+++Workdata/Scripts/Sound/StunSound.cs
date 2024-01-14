using UnityEngine;

public class StunSound : MonoBehaviour
{
    #region serialized fields
    [SerializeField] StunSubject stunSubject;
    [SerializeField] AudioSource audioSource;
    #endregion

    #region private fields

    #endregion

    void PlayStunSound(bool isStunned)
    {
        if (isStunned)
            SoundManager.Instance.PlaySound(SoundType.Stun, audioSource);
    }

    void OnEnable()
    {
        stunSubject.RegisterOnStun(PlayStunSound);
    }
    void OnDisable()
    {
        stunSubject.OnStun -= PlayStunSound;
    }
}