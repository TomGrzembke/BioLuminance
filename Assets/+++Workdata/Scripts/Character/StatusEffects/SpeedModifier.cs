using UnityEngine;

public class SpeedModifier : MonoBehaviour
{
    #region serialized fields
    [SerializeField] string note = "Comment for Overview";
    [SerializeField] SpeedSubject speedSubject;
    [SerializeField] float amount = -2;
    #endregion

    #region private fields
    bool subscribed;
    #endregion
    public void SetSpeedSubject(SpeedSubject _speedSubject)
    {
        speedSubject = _speedSubject;
        SubscribeModifier();
    }
    void OnEnable()
    {
        SubscribeModifier();
    }

    private void SubscribeModifier()
    {
        if (subscribed) return;
        if (speedSubject)
        {
            speedSubject.AddSpeedModifier(amount);
            subscribed = true;
        }
    }

    void OnDisable()
    {
        if (speedSubject)
            speedSubject.RemoveSpeedModifier(amount);
    }

}