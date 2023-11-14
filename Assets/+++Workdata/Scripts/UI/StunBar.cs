using UnityEngine;


public class StunBar : Bar
{
    #region serialized fields
    [SerializeField] Stun stun;
    #endregion

    void OnEnable()
    {
        stun.RegisterOnStunChangedAlpha(OnStunnedChanged, true);
    }

    void OnDisable()
    {
        stun.OnStunValueChangedAlpha -= OnStunnedChanged;
    }

    void OnStunnedChanged(float stunAmount)
    {
        bar.fillAmount = stunAmount;
    }
}