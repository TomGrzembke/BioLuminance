using UnityEngine;

public class TunaPassiveAbility : Ability
{
    #region serialized fields
    [SerializeField] SpeedModifier speedModifier;

    #endregion

    #region private fields

    #endregion

    private void OnEnable()
    {
        try
        {
            speedModifier = GetComponent<SpeedModifier>();
        }
        catch { }
    }
    protected override void ExecuteInternal()
    {
     
    }
    protected override void DeExecuteInternal()
    {
        speedModifier.enabled = false;
    }

    protected override void OnInitializedInternal(AbilitySlotManager _)
    {
        speedModifier.enabled = true;
        speedModifier.SetSpeedSubject(abilitySlotManager.StatusManager.SpeedSubject);
    }
}