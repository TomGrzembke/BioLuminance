using System.Collections.Generic;
using UnityEngine;

public class SpeedSubject : MonoBehaviour
{
    [SerializeField] float speed;
    public float Speed => speed;
    [SerializeField] float defaultSpeed;

    readonly List<float> SpeedModifierList = new();
    //readonly Dictionary<string, float> SpeedModifierDict = new();

    void Awake()
    {
        speed = defaultSpeed;
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
        float currentSpeed = defaultSpeed;
        for (int i = 0; i < SpeedModifierList.Count; i++)
        {
            currentSpeed += SpeedModifierList[i];
        }
        speed = currentSpeed;
    }

    public void RemoveSpeedModifier(float remSpeed)
    {
        if (SpeedModifierList.Contains(remSpeed))
            SpeedModifierList.Remove(speed);
        else
            print(remSpeed + " couldnt be removed at " + gameObject.name);

        CalculateSpeed();
    }
}