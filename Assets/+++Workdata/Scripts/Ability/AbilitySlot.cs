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

    public void Execute(bool deactivate = false)
    {
        if (currentAbility)
            currentAbility.Execute(deactivate);
    }
}