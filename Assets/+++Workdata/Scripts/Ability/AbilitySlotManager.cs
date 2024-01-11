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
    public static AbilitySlotManager Instance;
    #endregion

    void Awake() => Instance = this;

    void Start()
    {
        for (int i = 0; i < abilitySlots.Length; i++)
        {
            abilitySlots[i].SetSlotIndex(i);
            if (abilitySlots[i].CurrentAbility)
                AddNewAbility(abilitySlots[i].CurrentAbilityPrefab, i);
        }
    }

    public void ActivateSlot(int slotIndex, bool deactivate = false)
    {
        abilitySlots[slotIndex].Execute(deactivate);
    }

    public void AddNewAbility(GameObject newPrefab, int slotIndex)
    {
        abilitySlots[slotIndex].Execute(false);
        abilitySlots[slotIndex].ChangeAbilityPrefab(newPrefab, this);
    }

    public void AddNewAbility(GameObject newPrefab)
    {
        for (int i = 0; i < abilitySlots.Length; i++)
        {
            if (!abilitySlots[i].occupied)
            {
                abilitySlots[i].Execute(false);
                abilitySlots[i].ChangeAbilityPrefab(newPrefab, this);
                break;
            }
        }
    }
}