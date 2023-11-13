using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public event Action<float> OnHealthChanged;

    [SerializeField] float _maximumHealth = 10;
    [SerializeField] float _currentHealth = 10;

    public float CurrentHealth
    {
        get => _currentHealth;
        set => SetCurrentHealth(value);
    }

    void OnValidate()
    {
        SetCurrentHealth(_maximumHealth);
    }

    public void AddHealth(float additionalHealth)
    {
        SetCurrentHealth(_currentHealth + additionalHealth);
    }

    public void SetCurrentHealth(float newHealth)
    {
        if (newHealth > _maximumHealth)
            newHealth = _maximumHealth;

        var oldHealth = _currentHealth;
        _currentHealth = newHealth;

        OnHealthChanged?.Invoke(_currentHealth);
    }


    public void RegisterForOnHealthChanged(Action<float> callback, bool getInstantCallback = false)
    {
        OnHealthChanged += callback;
        if (getInstantCallback)
            callback(_currentHealth);
    }

    #region MaxHealth
    public event Action<float> OnMaximumHealthChanged;


    public float MaximumHealth
    {
        get => _maximumHealth;
        set => SetMaximumHealth(value);
    }

    public void SetMaximumHealth(float newMaximum)
    {
        _maximumHealth = newMaximum;
        OnMaximumHealthChanged?.Invoke(newMaximum);

        if (_currentHealth > newMaximum)
            SetCurrentHealth(newMaximum);
    }
    public void RegisterForOnMaximumHealthChanged(Action<float> callback, bool getInstantCallback = false)
    {
        OnMaximumHealthChanged += callback;
        if (getInstantCallback)
            callback(_maximumHealth);
    }
    #endregion
}
