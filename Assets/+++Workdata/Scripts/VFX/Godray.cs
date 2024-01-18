using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Random;

public class Godray : MonoBehaviour
{
    public float rotationSpeed = 1f;
    public float rotationOffset = 50f;
    float finalAngle;
    float randomOffset;
    float lightDimming;
    float randomIntensity;
    
    Light2D light2D;
    Vector3 startAngle;

    private void Start()
    {
        light2D = GetComponent<Light2D>();
        startAngle = transform.eulerAngles;

        randomOffset = Range(10, 30);
        randomOffset = Range(0, 2) == 1 ? -randomOffset : randomOffset;
        rotationSpeed = Range(0.1f, 0.5f);
        lightDimming = Range(1f, 3f);
        randomIntensity = Range(0.1f, 0.3f);
        float randomHeight = Range(100f, 200f);

        light2D.pointLightOuterRadius = randomHeight;
        light2D.intensity = randomIntensity;
    }

    private void Update()
    {
        Sway();
    }

    private void Sway()
    {
        finalAngle = startAngle.y + Mathf.Sin(Time.time * rotationSpeed) * rotationOffset;
        transform.eulerAngles =
            new Vector3(0, startAngle.y, 180 + finalAngle / randomOffset);

        light2D.intensity = Mathf.Abs(Mathf.Sin(Time.time / lightDimming) / 3.5f);
    }
}