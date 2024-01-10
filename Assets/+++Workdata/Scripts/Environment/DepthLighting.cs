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
    public Material mat;
    
    [Space(5)]
    public float clampedHeight;

    [Header("Lighting")] 
    public Light2D light2D;
    
    [Space(5)]
    public float lightLevel;
    public float maxLightLevel;
    public float minLightLevel;

    public Color ab;
    
    float subjectVerticalPosition;
    float maxHeight;
    float minHeight;

    private void Awake()
    {
        GetSpritePos();
    }

    private void Update()
    {
        subjectVerticalPosition = subject.transform.position.y;

        clampedHeight = subjectVerticalPosition;
        clampedHeight = Mathf.Clamp(clampedHeight, minHeight, maxHeight);

        LightInfo();
    }

    [ButtonMethod]
    public void GetSpritePos()
    {
        maxHeight = mapSprite.bounds.max.y;
        minHeight = mapSprite.bounds.min.y;
    }

    [ButtonMethod]
    public void Test()
    {
        mat.color = ab;
    }

    public void LightInfo()
    {
        if (clampedHeight < 0)
        {
            float percent = clampedHeight / 100;
            float testHeight = percent;
            if (testHeight < 0)
                testHeight *= -1f;
            lightLevel =+ maxLightLevel - testHeight;
        }
        
        lightLevel = Mathf.Clamp(lightLevel, minLightLevel, maxLightLevel);
        light2D.intensity = Mathf.Clamp(light2D.intensity, minLightLevel, maxLightLevel);
        
        light2D.intensity = lightLevel;
    }
}