using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpeedSubject : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float speed;
    public event Action<float> OnSpeedChanged;
    public float Speed
    {
        get
        {
            CalculateSpeed();
            return speed;
        }
    }
    [SerializeField] float defaultSpeed = 5;
    [SerializeField] float minSpeed = 0.5f;

    readonly List<float> SpeedModifierList = new();

    void Awake()
    {
        speed = defaultSpeed;
    }

    public void AddSpeedModifier(float addSpeed)
    {
        SpeedModifierList.Add(addSpeed);

        CalculateSpeed();
    }

    void CalculateSpeed()
    {
        float currentSpeed = defaultSpeed;
        for (int i = 0; i < SpeedModifierList.Count; i++)
        {
            currentSpeed += SpeedModifierList[i];
        }

        if (currentSpeed < minSpeed)
            currentSpeed = minSpeed;

        speed = currentSpeed;
        OnSpeedChanged?.Invoke(speed);
        if (agent)
            agent.speed = speed;
    }

    public void RemoveSpeedModifier(float remSpeed)
    {
        SpeedModifierList.Remove(remSpeed);
        CalculateSpeed();
    }

    public void ResetSpeed(float newDefault = 0)
    {
        SpeedModifierList.Clear();
        defaultSpeed = newDefault;
        CalculateSpeed();
    }

    public void SetDefault(float newDefault = 0)
    {
        defaultSpeed = newDefault;
        CalculateSpeed();
    }

    public void RegisterOnSpeedChanged(Action<float> callback, bool getInstantCallback = false)
    {
        OnSpeedChanged += callback;
        if (getInstantCallback)
            callback(Speed);
    }
}