using UnityEngine;

public class StingAbility : Ability
{
    #region serialized fields
    [SerializeField] GameObject stingerPrefab;
    [SerializeField] GameObject instantiated;
    [SerializeField] StingrayStinger stingrayStinger;
    [SerializeField] bool active;
    #endregion 

    #region private fields
    AbilitySlotManager slotManager;
    #endregion

    protected override void DeExecuteInternal()
    {

    }

    protected override void ExecuteInternal()
    {
        if (!active) return;
        stingrayStinger.AttackTarget(slotManager.MousePos);
    }

    protected override void OnInitializedInternal(AbilitySlotManager _abilitySlotManager)
    {
        slotManager = _abilitySlotManager;
        instantiated = Instantiate(stingerPrefab, slotManager.PlayerGFX.transform);
        stingrayStinger = instantiated.GetComponent<StingrayStinger>();
        stingrayStinger.SetOwnStatusManager(slotManager.StatusManager);
    }
}