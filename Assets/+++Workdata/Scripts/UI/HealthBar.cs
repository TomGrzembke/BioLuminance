using MyBox;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : Bar
{
    #region serialized fields
    [ConditionalField(nameof(healthLimb), true), SerializeField] HealthSubject health;
    [ConditionalField(nameof(health), true), SerializeField] LimbSubject healthLimb;
    [SerializeField] Image afterShockBar;
    [ConditionalField(nameof(afterShockBar)), Tooltip("Multiplier of the delta time lerp modifier")]
    [SerializeField] float afterShockSpeed = 3;
    #endregion

    #region private fields
    Coroutine afterShockCoroutine;
    #endregion

    void Start()
    {
        afterShockBar.fillAmount = 1;
    }

    void OnEnable()
    {
        if (health != null)
            health.RegisterOnHealthChangedAlpha(OnHealthChanged, true);
        else if(healthLimb != null)
            healthLimb.RegisterOnHealthChangedAlpha(OnHealthChanged, true);
    }

    void OnDisable()
    {
        if (health != null)
            health.OnHealthChangedAlpha -= OnHealthChanged;
        else if(healthLimb != null)
            healthLimb.OnHealthChangedAlpha -= OnHealthChanged;
    }

    void OnHealthChanged(float health)
    {
        bar.fillAmount = health;

        AfterShockLogic(health);
    }

    void AfterShockLogic(float health)
    {
        if (afterShockBar == null) return;
        if (!gameObject.activeSelf) return;

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