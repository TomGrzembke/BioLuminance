using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DepthLighting : MonoBehaviour
{
    public SpriteRenderer mapSprite;
    public float spriteVerticalSize;
    public GameObject subject;
    public float subjectVerticalPosition;
    
    [Header("Lighting")] 
    public Light2D light2D;
    public float maxLightingLevel;
    public float minLightingLevel;
    
    private void Awake()
    {
        spriteVerticalSize = (mapSprite.bounds.extents.x);
    }

    private void Update()
    {
        subjectVerticalPosition = subject.transform.position.y;
    }
}