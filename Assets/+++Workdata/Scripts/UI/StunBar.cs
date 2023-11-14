using UnityEngine;


public class StunBar : Bar
{
    #region serialized fields
    [SerializeField] Stun stun;
    [SerializeField] Color stunCol;
    [SerializeField] Color defaultCol;
    #endregion

    void OnEnable()
    {
        stun.RegisterOnStunChangedAlpha(OnStunnedChanged, true);
        stun.RegisterOnStun(OnStun);
    }

    void OnDisable()
    {
        stun.OnStunValueChangedAlpha -= OnStunnedChanged;
        stun.OnStun -= OnStun;
    }

    void OnStunnedChanged(float stunAmount)
    {
        bar.fillAmount = stunAmount;
    }

    void OnStun(bool condition)
    {
        bar.color = condition ? stunCol : defaultCol;
    }
}