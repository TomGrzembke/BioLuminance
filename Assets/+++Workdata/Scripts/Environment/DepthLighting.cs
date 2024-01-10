using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class DepthLighting : MonoBehaviour
{
    public SpriteRenderer mapSprite;
    public GameObject subject;
    public float subjectVerticalPosition;
    [Space(5)] public float maxHeight;
    public float minHeight;
    public float clampedHeight;
    public float testHeight;

    [Header("Lighting")] public Light2D light2D;
    public float lightLevel;
    public float maxLightLevel;
    public float minLightLevel;

    private void Awake()
    {
        GetSpritePos();
    }

    private void Update()
    {
        subjectVerticalPosition = subject.transform.position.y;

        clampedHeight = subjectVerticalPosition;
        clampedHeight = Mathf.Clamp(clampedHeight, minHeight, maxHeight);
        
        if (clampedHeight < 0)
        {
            float percent = clampedHeight / 100;
            testHeight = percent;
            if (testHeight < 0)
                testHeight *= -1f;
        }

        LightInfo();
    }

    [ButtonMethod]
    public void GetSpritePos()
    {
        maxHeight = mapSprite.bounds.max.y;
        minHeight = mapSprite.bounds.min.y;
    }

    public void LightInfo()
    {
        lightLevel = Mathf.Clamp(lightLevel, minLightLevel, maxLightLevel);
        light2D.intensity = Mathf.Clamp(light2D.intensity, minLightLevel, maxLightLevel);


        light2D.intensity = lightLevel;
    }
}