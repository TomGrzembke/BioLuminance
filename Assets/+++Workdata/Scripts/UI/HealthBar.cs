using UnityEngine;

public class HealthBar : Bar
{
    #region serialized fields
    [SerializeField] Health health;
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
    }
}