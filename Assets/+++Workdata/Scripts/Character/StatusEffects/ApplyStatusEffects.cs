using System;
using UnityEngine;

public class ApplyStatusEffects : MonoBehaviour
{
    #region serialized fields
    [SerializeField] bool player;
    [SerializeField] StunEffectCondition stunEffectCondition;
    #endregion

    #region private fields

    #endregion

    public void ApplyEffects(StatusEffects _statusEffects, StatusManager targetStatusManager, LimbSubject limbSubject = null, StatusManager ownStatusManager = null)
    {
        bool hasDoneSmth = false;

        if (_statusEffects.damagePerInstance != 0 && limbSubject)
        {
            limbSubject.AddDamage(_statusEffects.damagePerInstance);
            hasDoneSmth = true;
        }

        if (_statusEffects.stunPerInstance != 0)
        {
            targetStatusManager.AddStun(_statusEffects.stunPerInstance * stunEffectCondition.Calc_percentageDebuff);
            hasDoneSmth = true;
        }

        if (_statusEffects.speedModifier)
        {
            targetStatusManager.AddSpeedModifier(_statusEffects.speedModifier);
            hasDoneSmth = true;
        }

        if (!hasDoneSmth) return;

        if(player)
            CombatManager.Instance.CreatureInteraction(targetStatusManager);
        else if(targetStatusManager.IsPlayer)
            CombatManager.Instance.CreatureInteraction(ownStatusManager);
    }

    public void ApplyEffects(StatusEffects _statusEffects, LimbSubject limbSubject, StatusManager ownStatusManager = null)
    {
        ApplyEffects(_statusEffects, limbSubject.ownStatusManager, limbSubject, ownStatusManager);
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