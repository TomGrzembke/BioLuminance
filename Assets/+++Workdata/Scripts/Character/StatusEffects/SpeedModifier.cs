using UnityEngine;

public class SpeedModifier : MonoBehaviour
{
    #region serialized fields
    [SerializeField] string note = "Comment for Overview";
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