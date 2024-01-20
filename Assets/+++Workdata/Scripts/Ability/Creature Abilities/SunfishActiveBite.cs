using UnityEngine;

public class SunfishActiveBite : Ability
{
    #region serialized fields
    [SerializeField] GameObject bitePrefab;
    #endregion

    #region private fields
    [SerializeField] GameObject instantiated;
    [SerializeField] BiteLogic biteLogic;
    #endregion
    protected override void DeExecuteInternal()
    {

    }

    protected override void ExecuteInternal()
    {
        if (biteLogic == null)
            biteLogic = instantiated.GetComponent<BiteLogic>();

            biteLogic.Bite();
    }

    protected override void OnInitializedInternal(AbilitySlotManager _abilitySlotManager)
    {
        instantiated = Instantiate(bitePrefab, _abilitySlotManager.PlayerGFX.transform);
        biteLogic = instantiated.GetComponent<BiteLogic>();
        biteLogic.SetOwnStatusManager(_abilitySlotManager.StatusManager);
    }


}