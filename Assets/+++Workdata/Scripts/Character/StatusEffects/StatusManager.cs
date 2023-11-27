using UnityEngine;

public class StatusManager : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Transform pos;
    [SerializeField] StunSubject stunSubject;
    [SerializeField] HealthSubject healthSubject;
    [SerializeField] SpeedSubject speedSubject;
    #endregion

    #region private fields

    #endregion

    public void AddStun(float additionalStun)
    {
        stunSubject.AddStun(additionalStun);
    }
}