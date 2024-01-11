using System;
using UnityEngine;

public class ApplyStatusEffects : MonoBehaviour
{
    #region serialized fields
    [SerializeField] CombatManager combatManager;
    [SerializeField] bool player;
    #endregion

    #region private fields

    #endregion

    public void ApplyEffects(StatusEffects _statusEffects, StatusManager targetStatusManager, LimbSubject limbSubject = null)
    {
        bool hasDoneSmth = false;

        if (_statusEffects.damagePerInstance != 0 && limbSubject)
        {
            limbSubject.AddDamage(_statusEffects.damagePerInstance);
            hasDoneSmth = true;
        }

        if (_statusEffects.stunPerInstance != 0)
        {
            targetStatusManager.AddStun(_statusEffects.stunPerInstance);
            hasDoneSmth = true;
        }

        if (_statusEffects.speedModifier)
        {
            targetStatusManager.AddSpeedModifier(_statusEffects.speedModifier);
            hasDoneSmth = true;
        }

        if (!combatManager || !hasDoneSmth) return;
        if(player)
            combatManager.CreatureInteraction(targetStatusManager);
        else
            combatManager.CreatureInteraction(targetStatusManager);

    }

    public void ApplyEffects(StatusEffects _statusEffects, LimbSubject limbSubject)
    {
        ApplyEffects(_statusEffects, limbSubject.ownStatusManager, limbSubject);
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