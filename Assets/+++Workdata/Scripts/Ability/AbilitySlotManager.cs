using UnityEngine;

public class AbilitySlotManager : MonoBehaviour
{
    #region serialized fields
    [SerializeField] GameObject playerObj;
    [SerializeField] AbilitySlot[] abilitySlots;
    #endregion

    #region private fields

    #endregion
    public void ActivateSlot(int slotIndex, bool deactivate = false)
    {
        abilitySlots[slotIndex].Execute(deactivate);
    }

    public void AddNewAbility(GameObject newPrefab,int slotIndex)
    {
        abilitySlots[slotIndex].ChangeAbilityPrefab(newPrefab);
    }
}