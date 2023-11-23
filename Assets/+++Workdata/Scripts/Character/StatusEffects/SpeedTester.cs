using UnityEngine;

public class SpeedTester : MonoBehaviour
{
    #region serialized fields
    [SerializeField] SpeedSubject speedSubject;
    [SerializeField] float amount = -2;
    #endregion

    #region private fields

    #endregion

    private void OnEnable()
    {
        speedSubject.AddSpeedModifier(amount);
    }

    private void OnDisable()
    {
        speedSubject.RemoveSpeedModifier(amount);
    }

}