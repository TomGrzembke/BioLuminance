using UnityEngine;

public class StatusManager : MonoBehaviour
{
    #region serialized fields
    public Transform Trans => trans;
    [SerializeField] Transform trans;
    public StunSubject StunSunject => stunSubject;
    [SerializeField] StunSubject stunSubject;
    public HealthSubject HealthSubject => healthSubject;
    [SerializeField] HealthSubject healthSubject;
    public SpeedSubject SpeedSubject => speedSubject;
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