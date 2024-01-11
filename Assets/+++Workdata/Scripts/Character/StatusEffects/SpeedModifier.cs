using UnityEngine;

public class SpeedModifier : MonoBehaviour
{
    #region serialized fields
#pragma warning disable 414
    [SerializeField] string note = "Comment for Overview";
#pragma warning restore 414

    [SerializeField] SpeedSubject speedSubject;
    [SerializeField] float amount = -2;
    public float Amount => amount;
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