using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthSubject : MonoBehaviour
{
    public event Action<float> OnHealthChanged;
    public event Action<float> OnHealthChangedAlpha;
    public event Action<CreatureLogic> OnCreatureDied;
    [SerializeField] List<LimbSubject> limbSubjects = new();

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

    [ButtonMethod]
    public void GatherLimbs()
    {
        limbSubjects = transform.parent.GetComponentsInChildren<LimbSubject>().ToList();
    }

    public void AddHealth(float additionalHealth)
    {
        CalculateLimbHealth();

        SetCurrentHealth(currentHealth + additionalHealth);
    }

    void CalculateLimbHealth()
    {
        if (limbSubjects.Count == 0) return;
        float limbHealth = 0;
        for (int i = 0; i < limbSubjects.Count; i++)
        {
            limbHealth += limbSubjects[i].CurrentHealth;
        }
        currentHealth = limbHealth;
    }

    public void SetCurrentHealth(float newHealth)
    {
        newHealth = Mathf.Clamp(newHealth, 0f, maximumHealth);

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
