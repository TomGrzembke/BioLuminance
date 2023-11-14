using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public event Action<float> OnHealthChanged;
    public event Action<float> OnHealthChangedAlpha;

    [SerializeField] float maximumHealth = 10;
    [SerializeField] float currentHealth = 10;

    public float CurrentHealth
    {
        get => currentHealth;
        set => SetCurrentHealth(value);
    }

    void OnValidate()
    {
        SetCurrentHealth(maximumHealth);
    }

    public void AddHealth(float additionalHealth)
    {
        SetCurrentHealth(currentHealth + additionalHealth);
    }

    public void SetCurrentHealth(float newHealth)
    {
        if (newHealth > maximumHealth)
            newHealth = maximumHealth;
        else if (currentHealth + newHealth < 0)
            newHealth = 0;

        var oldHealth = currentHealth;
        currentHealth = newHealth;

        OnHealthChanged?.Invoke(currentHealth);
        OnHealthChangedAlpha?.Invoke(currentHealth / maximumHealth);
    }


    public void RegisterOnHealthChanged(Action<float> callback, bool getInstantCallback = false)
    {
        OnHealthChanged += callback;
        if (getInstantCallback)
            callback(currentHealth);
    }

    public void RegisterOnHealthChangedAlpha(Action<float> callback, bool getInstantCallback = false)
    {
        OnHealthChangedAlpha += callback;
        if (getInstantCallback)
            callback(currentHealth / maximumHealth);
    }

    #region MaxHealth
    public event Action<float> OnMaximumHealthChanged;


    public float MaximumHealth
    {
        get => maximumHealth;
        set => SetMaximumHealth(value);
    }

    public void SetMaximumHealth(float newMaximum)
    {
        maximumHealth = newMaximum;
        OnMaximumHealthChanged?.Invoke(newMaximum);

        if (currentHealth > newMaximum)
            SetCurrentHealth(newMaximum);
    }
    public void RegisterForOnMaximumHealthChanged(Action<float> callback, bool getInstantCallback = false)
    {
        OnMaximumHealthChanged += callback;
        if (getInstantCallback)
            callback(maximumHealth);
    }
    #endregion
}
