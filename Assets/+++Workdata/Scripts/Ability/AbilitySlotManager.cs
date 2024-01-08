using UnityEngine;

public class AbilitySlotManager : MonoBehaviour
{
    #region serialized fields
    [SerializeField] GameObject playerObj;
    [SerializeField] StatusManager statusManager;
    public StatusManager StatusManager => statusManager;
    [SerializeField] AbilitySlot[] abilitySlots;
    #endregion

    #region private fields

    #endregion

    void Start()
    {
        for (int i = 0; i < abilitySlots.Length; i++)
        {
            if (abilitySlots[i].CurrentAbility)
                abilitySlots[i].EnterAbility(this);
        }
    }

    public void ActivateSlot(int slotIndex, bool deactivate = false)
    {
        abilitySlots[slotIndex].Execute(deactivate);
    }

    public void AddNewAbility(GameObject newPrefab, int slotIndex)
    {
        abilitySlots[slotIndex].Execute(false);
        abilitySlots[slotIndex].ChangeAbilityPrefab(newPrefab);
        abilitySlots[slotIndex].EnterAbility(this);
    }
}