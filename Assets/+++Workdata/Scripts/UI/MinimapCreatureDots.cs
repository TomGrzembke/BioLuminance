using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MinimapCreatureDots : MonoBehaviour
{
    [SerializeField] List<GameObject> mapGameObjects;

    void Start()
    {
        mapGameObjects = GetGameObjectsInLayer("Map");
    }

    public List<GameObject> GetGameObjectsInLayer(string layerName)
    {
        List<GameObject> allObjectsInLayer = new List<GameObject>();

        // Find all objects of type GameObject in the scene, including inactive ones
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        // Filter objects based on layer and activity
        allObjectsInLayer = allObjects.Where(obj => obj.layer == LayerMask.NameToLayer(layerName) && obj.activeInHierarchy).ToList();

        Debug.Log($"<color=blue>There are {allObjectsInLayer.Count} items on the {layerName} layer.</color>");

        return allObjectsInLayer;
    }
}