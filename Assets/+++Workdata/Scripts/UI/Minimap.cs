using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] StateManager stateManager;
    [SerializeField] SpriteRenderer mapSpriteRenderer;

    public float indicatorSize = 1f;
    Vector2 indicatorSizeVec;

    private void Awake()
    {
        stateManager = GetComponentInChildren<StateManager>();
    }

    private void Start()
    {
        mapSpriteRenderer = GetSpriteRendererInLayer(gameObject, "Map");
    }

    private void OnValidate()
    {
        indicatorSizeVec.x = indicatorSize;
        indicatorSizeVec.y = indicatorSize;
        mapSpriteRenderer.transform.localScale = indicatorSizeVec;
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
        if (stateManager.currentState)
            mapSpriteRenderer.color = new Color(255, 0, 0);
        else
            mapSpriteRenderer.color = new Color(255, 255, 255);
    }
}