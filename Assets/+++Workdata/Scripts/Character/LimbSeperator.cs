using UnityEngine;

public class LimbSeperator : MonoBehaviour
{
    #region serialized fields
    [SerializeField] LimbSubject limbSubject;
    [SerializeField] Transform gfx;
    [SerializeField] Transform creatureHeader;
    [SerializeField] Transform[] childrenToTakeWith;
    [SerializeField] ParticleSystem[] bloodToPlayPS;
    #endregion

    void OnLimbDied(bool died)
    {
        if (!died) return;

        for (int i = 0; i < bloodToPlayPS.Length; i++)
        {
            bloodToPlayPS[i].Play();
        }

        for (int i = 0; i < childrenToTakeWith.Length; i++)
        {
            childrenToTakeWith[i].SetParent(gfx);
        }

        gfx.SetParent(creatureHeader);

    }

    void OnEnable()
    {
        limbSubject.RegisterOnLimbDied(OnLimbDied);
    }

    void OnDisable()
    {
        limbSubject.OnLimbDied -= OnLimbDied;
    }
}