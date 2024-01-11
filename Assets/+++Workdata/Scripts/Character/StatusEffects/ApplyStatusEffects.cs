using System;
using UnityEngine;

public class ApplyStatusEffects : MonoBehaviour
{
    #region serialized fields

    #endregion

    #region private fields

    #endregion

    public void ApplyEffects(StatusEffects _statusEffects, StatusManager targetStatusManager, LimbSubject limbSubject = null)
    {
        if (_statusEffects.damagePerInstance != 0 && limbSubject)
            limbSubject.AddDamage(_statusEffects.damagePerInstance);

        if (_statusEffects.stunPerInstance != 0)
            targetStatusManager.AddStun(_statusEffects.stunPerInstance);

        if (_statusEffects.speedModifier)
            targetStatusManager.AddSpeedModifier(_statusEffects.speedModifier);
    }

    public void ApplyEffects(StatusEffects _statusEffects, LimbSubject limbSubject)
    {
        if (_statusEffects.damagePerInstance != 0 && limbSubject)
            limbSubject.AddDamage(_statusEffects.damagePerInstance);

        if (_statusEffects.stunPerInstance != 0)
            limbSubject.ownStatusManager.AddStun(_statusEffects.stunPerInstance);

        if (_statusEffects.speedModifier)
            limbSubject.ownStatusManager.AddSpeedModifier(_statusEffects.speedModifier);
    }

    public void RemoveSpeedModifier(StatusManager targetStatusManager, SpeedModifier speedModifier)
    {
        if (speedModifier)
            targetStatusManager.RemoveSpeedModifier(speedModifier);
    }

}

[Serializable]
public struct StatusEffects
{
    public float damagePerInstance;
    public float stunPerInstance;
    public SpeedModifier speedModifier;
}