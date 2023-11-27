using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] SpriteRenderer mapSpriteRenderer;
    StateManager stateManager;

    private void Awake()
    {
        stateManager = GetComponent<StateManager>();
    }

    private void Start()
    {
        mapSpriteRenderer = GetSpriteRendererInLayer(gameObject, "Map");
    }

    private void Update()
    {
        HandleMapIndicators();
    }

    public SpriteRenderer GetSpriteRendererInLayer(GameObject parent, string layerName)
    {
        SpriteRenderer spriteRendererInLayer = null;

        // Get all child objects of the parent GameObject
        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);

        // Find the first child object that matches the layer, is active, and has a SpriteRenderer component
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
        /*
        if (stateManager.currentState == chaseState)
            if (mapSpriteRenderer != null)
                mapSpriteRenderer.color = new Color(255, 0, 0);
         */
    }
}