using UnityEngine;

public class StatusManager : MonoBehaviour
{
    #region serialized fields
    public Transform Trans => trans;
    [SerializeField] Transform trans;
    [SerializeField] StunSubject stunSubject;
    [SerializeField] HealthSubject healthSubject;
    [SerializeField] SpeedSubject speedSubject;
    #endregion

    #region private fields

    #endregion

    void Awake()
    {
        trans = transform;
    }

    public void AddStun(float additionalStun)
    {
        stunSubject.AddStun(additionalStun);
    }

    public void AddHealth(float additionalHealth)
    {
        healthSubject.AddHealth(additionalHealth);
    }
}