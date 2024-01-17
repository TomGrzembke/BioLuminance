using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    #region serialized fields
    [SerializeField] protected AbilitySO abilitySO;
    public AbilitySO AbilitySO => abilitySO;
    #endregion

    #region private fields
    protected AbilitySlotManager abilitySlotManager;

    #endregion
    public void EnterAbility(AbilitySlotManager _abilitySlotManager)
    {
        abilitySlotManager = _abilitySlotManager;
        OnInitialized(_abilitySlotManager);
    }

    public virtual void Execute(AbilitySlotManager _abilitySlotManager, bool deactivate = false)
    {
        abilitySlotManager = _abilitySlotManager;
        if (!deactivate)
            ExecuteInternal();
        else
            DeExecuteInternal();
    }
    public void OnInitialized(AbilitySlotManager _abilitySlotManager)
    {
        OnInitializedInternal(_abilitySlotManager);
    }
    protected abstract void ExecuteInternal();
    protected abstract void DeExecuteInternal();
    protected abstract void OnInitializedInternal(AbilitySlotManager _abilitySlotManager);
}