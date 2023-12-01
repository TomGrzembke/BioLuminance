using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

public class Minimap : MonoBehaviour
{
    [SerializeField] StateManager stateManager;
    [SerializeField] SpriteRenderer mapSpriteRenderer;
    [SerializeField] SpriteRenderer creatureRenderer;
    
    public Vector2 spriteSizeVec;

    
    [Flags]
    public enum Flags
    {
        none = 0,
        North = 1,
        NorthEast = 2,
        NorthWest = 4,
        East = 8,
        EastWest = 16,
        EastSouth = 32,
        South = 64,
        SouthEast = 128,
        SouthWest = 256,
        West = 512,
        WestSouth = 1024,
        WestNorth = 2048,
    }

    public Flags flags;

    private void Awake()
    {
        stateManager = GetComponentInChildren<StateManager>();
        mapSpriteRenderer = GetSpriteRendererInLayer(gameObject, "Map");
    }

    private void FixedUpdate()
    {
        HandleMapIndicators();
    }

    SpriteRenderer GetSpriteRendererInLayer(GameObject parent, string layerName)
    {
        SpriteRenderer spriteRendererInLayer = null;
        
        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);
        
        foreach (Transform child in allChildren)
        {
            SpriteRenderer childSpriteRenderer = child.GetComponent<SpriteRenderer>();
            if (childSpriteRenderer != null && child.gameObject.layer == LayerMask.NameToLayer(layerName))
            {
                spriteRendererInLayer = childSpriteRenderer;
                break;
            }
        }

        return spriteRendererInLayer;
    }

    public void HandleMapIndicators()
    {
        if (stateManager.currentState.Dangerous)
            mapSpriteRenderer.color = new Color(255, 0, 0);
        else
            mapSpriteRenderer.color = new Color(255, 255, 255);
    }
}