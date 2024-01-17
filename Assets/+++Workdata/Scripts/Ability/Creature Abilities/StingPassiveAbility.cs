using System.Collections;
using UnityEngine;

public class StingPassiveAbility : Ability
{
    #region serialized fields
    [SerializeField] GameObject stingerPrefab;
    #endregion

    #region private fields

    #endregion

    protected override void DeExecuteInternal()
    {

    }

    protected override void ExecuteInternal()
    {

    }

    protected override void OnInitializedInternal(AbilitySlotManager _abilitySlotManager)
    {
        GameObject instantiated = Instantiate(stingerPrefab, _abilitySlotManager.PlayerGFX.transform);
        instantiated.GetComponent<StingrayStinger>().SetOwnStatusManager(_abilitySlotManager.StatusManager);
    }
}