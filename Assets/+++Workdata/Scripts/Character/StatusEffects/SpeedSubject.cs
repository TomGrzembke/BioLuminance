using System.Collections.Generic;
using UnityEngine;

public class SpeedSubject : MonoBehaviour
{
    [SerializeField] float speed;
    public float Speed => speed;
    [SerializeField] float defaultSpeed = 5;

    readonly List<float> SpeedModifierList = new();

    void Awake()
    {
        AddSpeedModifier(defaultSpeed);
    }
    public float GetCurrentSpeed()
    {
        float currentSpeed = defaultSpeed;
        for (int i = 0; i < SpeedModifierList.Count; i++)
        {
            currentSpeed += SpeedModifierList[i];
        }
        return currentSpeed;
    }

    public void AddSpeedModifier(float addSpeed)
    {
        SpeedModifierList.Add(addSpeed);

        CalculateSpeed();
    }

    void CalculateSpeed()
    {
        float currentSpeed = 0;
        for (int i = 0; i < SpeedModifierList.Count; i++)
        {
            currentSpeed += SpeedModifierList[i];
        }
        speed = currentSpeed;
    }

    public void RemoveSpeedModifier(float remSpeed)
    {
        if (!SpeedModifierList.Remove(remSpeed))
            print(remSpeed + " couldnt be removed at " + gameObject.name);

        CalculateSpeed();
    }
}