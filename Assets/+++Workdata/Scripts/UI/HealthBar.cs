using MyBox;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : Bar
{
    #region serialized fields
    [SerializeField] HealthSubject health;
    [SerializeField] Image afterShockBar;
    [ConditionalField(nameof(afterShockBar)), Tooltip("Multiplier of the delta time lerp modifier")]
    [SerializeField] float afterShockSpeed = 3;
    #endregion

    #region private fields
    Coroutine afterShockCoroutine;
    #endregion

    void OnEnable()
    {
        health.RegisterOnHealthChangedAlpha(OnHealthChanged, true);
    }

    void OnDisable()
    {
        health.OnHealthChangedAlpha -= OnHealthChanged;
    }

    void OnHealthChanged(float health)
    {
        bar.fillAmount = health;

        AfterShockLogic(health);
    }

    private void AfterShockLogic(float health)
    {
        if (afterShockBar == null) return;

        if (afterShockCoroutine == null)
            afterShockCoroutine = StartCoroutine(AfterShockbarCo(health));
        else
        {
            StopCoroutine(afterShockCoroutine);
            afterShockCoroutine = StartCoroutine(AfterShockbarCo(health));
        }
    }

    IEnumerator AfterShockbarCo(float health)
    {
        while (afterShockBar.fillAmount != bar.fillAmount)
        {
            afterShockBar.fillAmount = Mathf.LerpUnclamped(afterShockBar.fillAmount, health, Time.deltaTime * afterShockSpeed);
            yield return null;
        }

    }
}