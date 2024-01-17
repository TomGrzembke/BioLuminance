using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class Godrays : MonoBehaviour
{
    public float spawnRange;
    public GameObject[] godRayArray;
    
    float finalAngle;
    Vector3 startAngle; // Reference to the object's original angle values
    
    public Vector2 plusX;
    public Vector2 minusX;

    public float randomIntensity;

    private void Awake()
    {
        SpawnRays();
    }

    private void OnValidate()
    {
        plusX = new Vector2(spawnRange / 2, transform.localPosition.y);
        minusX = new Vector2(spawnRange / -2, transform.localPosition.y);
    }

    [ButtonMethod]
    public void SpawnRays()
    {
        plusX = new Vector2(spawnRange / 2, transform.localPosition.y);
        minusX = new Vector2(spawnRange / -2, transform.localPosition.y);
        
        for (int i = 0; i < godRayArray.Length; i++)
        {
            var spawnpoint = Random.Range(plusX.x, minusX.x);
            var g = Instantiate(godRayArray[i], new Vector3(spawnpoint, transform.localPosition.y), new Quaternion(0,0,180,0));
            g.transform.parent = gameObject.transform;

            foreach (Transform child in transform)
            {
                var light = child.GetComponent<Light2D>();
                
                randomIntensity = Random.Range(0.1f, 0.3f);
                float randomHeight = Random.Range(100f, 200f);

                light.pointLightOuterRadius = randomHeight;
                light.intensity = randomIntensity;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(plusX, minusX);
    }
}