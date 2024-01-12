using System.Collections;
using UnityEngine;

public class HealthRegenCondition : Condition
{
    #region serialized fields
    [SerializeField] LimbSubject limbSubject;
    [SerializeField] float regenPerTick = .1f;
    [SerializeField] float interval = .5f;
    #endregion

    #region private fields
    Coroutine regenRoutine;
    #endregion

    void OnEnable()
    {
        regenRoutine = StartCoroutine(LifeRegen());
    }

    void OnDisable()
    {
        StopCoroutine(regenRoutine);
    }

    IEnumerator LifeRegen()
    {
        yield return new WaitForSeconds(interval);

        limbSubject.AddHealth(regenPerTick * percentageEffectiveness / 100);

        regenRoutine = StartCoroutine(LifeRegen());
    }
}