using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    #region serialized fields
    [SerializeField] GameObject currentAbilityPrefab;
    [SerializeField] Image abilityImage;
    #endregion

    #region private fields
    Ability currentAbility;
    public Ability CurrentAbility => currentAbility;
    AbilitySlotManager abilitySlotManager;
    #endregion

    void OnValidate()
    {
        RefreshPicture();
    }
    void RefreshPicture()
    {
        if (currentAbilityPrefab)
            currentAbilityPrefab.TryGetComponent(out currentAbility);

        abilityImage.sprite = currentAbilityPrefab ? currentAbility.AbilitySprite : null;
    }

    public void ChangeAbilityPrefab(GameObject newAbilityPrefab)
    {
        currentAbilityPrefab = newAbilityPrefab;
        RefreshPicture();
    }
    public void EnterAbility(AbilitySlotManager _abilitySlotManager)
    {
        abilitySlotManager = _abilitySlotManager;
        if (currentAbility)
            currentAbility.EnterAbility(abilitySlotManager);
    }
    public void Execute(bool deactivate = false)
    {
        if (currentAbility)
            currentAbility.Execute(abilitySlotManager, deactivate);
    }
}