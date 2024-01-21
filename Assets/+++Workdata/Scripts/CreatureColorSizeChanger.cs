using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreatureColorSizeChanger : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    float randomSpriteScale;

    private byte randomR;
    private byte randomG;
    private byte randomB;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        randomSpriteScale = Random.Range(0.7f, 1.5f);
        randomR = (byte)Random.Range(150, 255);
        randomG = (byte)Random.Range(150, 255);
        randomB = (byte)Random.Range(150, 255);
    }

    private void Start()
    {
        gameObject.transform.localScale = new Vector3(randomSpriteScale, randomSpriteScale);
        spriteRenderer.color = new Color32(randomR, randomG, randomB, 255);
    }
}