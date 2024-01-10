using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthSubject : MonoBehaviour
{
    public event Action<float> OnHealthChanged;
    public event Action<float> OnHealthChangedAlpha;
    public event Action<bool> OnCreatureDied;
    [SerializeField] List<LimbSubject> limbSubjects = new();

    float maximumHealth = 10;
    [SerializeField] float currentHealth = 10;

    public float CurrentHealth
    {
        get => currentHealth;
        set => SetCurrentHealth(value);
    }

    void Awake()
    {
        CalculateMaxHealth();
    }

    void OnValidate()
    {
        CalculateMaxHealth();
        SetCurrentHealth(maximumHealth);
        if (limbSubjects.Count == 0)
            GatherLimbs();
    }

    void CalculateMaxHealth()
    {
        maximumHealth = 0;
        for (int i = 0; i < limbSubjects.Count; i++)
        {
            maximumHealth += limbSubjects[i].MaximumHealth;
        }
    }

    [ButtonMethod]
    public void GatherLimbs()
    {
        limbSubjects = transform.parent.GetComponentsInChildren<LimbSubject>().ToList();
        CalculateMaxHealth();
    }

    public void AddHealth(float additionalHealth)
    {
        CalculateLimbHealth();

        SetCurrentHealth(currentHealth + additionalHealth);
    }

    public void CalculateLimbHealth(float _ = 0)
    {
        if (limbSubjects.Count == 0) return;
        float limbHealth = 0;
        for (int i = 0; i < limbSubjects.Count; i++)
        {
            limbHealth += limbSubjects[i].CurrentHealth;
        }
        SetCurrentHealth(limbHealth);
    }

    public void SetCurrentHealth(float newHealth)
    {
        CalculateMaxHealth();
        newHealth = Mathf.Clamp(newHealth, 0f, maximumHealth);

        currentHealth = newHealth;

        OnHealthChanged?.Invoke(currentHealth);
        OnHealthChangedAlpha?.Invoke(currentHealth / maximumHealth);

        if (currentHealth <= 0)
            OnCreatureDied?.Invoke(true);
    }

    public void RegisterOnHealthChanged(Action<float> callback, bool getInstantCallback = false)
    {
        OnHealthChanged += callback;
        if (getInstantCallback)
            callback(currentHealth);
    }

    public void RegisterOnAliveChanged(Action<bool> callback)
    {
        OnCreatureDied += callback;
    }

    public void RegisterOnHealthChangedAlpha(Action<float> callback, bool getInstantCallback = false)
    {
        OnHealthChangedAlpha += callback;
        if (getInstantCallback)
            callback(currentHealth / maximumHealth);
    }

    void OnEnable()
    {
        for (int i = 0; i < limbSubjects.Count; i++)
        {
            limbSubjects[i].RegisterOnHealthChanged(CalculateLimbHealth);
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < limbSubjects.Count; i++)
        {
            limbSubjects[i].OnHealthChangedAlpha -= CalculateLimbHealth;
        }
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
        if (newMaximum == maximumHealth) return;

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
