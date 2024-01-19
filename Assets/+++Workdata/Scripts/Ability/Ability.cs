using System.Collections;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    #region serialized fields
    [SerializeField] protected float cooldown;
    [SerializeField] protected AbilitySO abilitySO;
    public AbilitySO AbilitySO => abilitySO;
    #endregion

    #region private fields
    protected AbilitySlotManager abilitySlotManager;
    Coroutine coolDownCor;
    #endregion
    public void EnterAbility(AbilitySlotManager _abilitySlotManager)
    {
        abilitySlotManager = _abilitySlotManager;
        OnInitialized(_abilitySlotManager);
    }

    public virtual void Execute(AbilitySlotManager _abilitySlotManager, bool deactivate = false)
    {
        if (coolDownCor != null) return;

        abilitySlotManager = _abilitySlotManager;
        if (!deactivate)
        {
            coolDownCor = StartCoroutine(Cooldown());
            ExecuteInternal();
        }
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

    IEnumerator Cooldown()
    {
        float wentByTime = 0;
        while (wentByTime < cooldown)
        {
            wentByTime += Time.deltaTime;
            yield return null;
        }

        coolDownCor = null;
    }
}