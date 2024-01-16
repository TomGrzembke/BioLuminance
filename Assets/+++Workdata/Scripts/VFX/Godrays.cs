using MyBox;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class Godrays : MonoBehaviour
{
    public float spawnRange;
    public GameObject[] godRayArray;

    Vector2 plusX;
    Vector2 minusX;

    void Awake()
    {
        throw new NotImplementedException();
    }

    [ButtonMethod]
    public void SpawnRays()
    {
        plusX = new Vector2(spawnRange / 2, transform.localPosition.y);
        minusX = new Vector2(spawnRange / -2, transform.localPosition.y);

        for (int i = 0; i < godRayArray.Length; i++)
        {
            var spawnpoint = Random.Range(plusX.x, minusX.x);
            var g = Instantiate(godRayArray[i], new Vector3(spawnpoint, transform.localPosition.y), new Quaternion(0, 0, 180, 0));
            g.transform.parent = gameObject.transform;

            foreach (Transform child in transform)
            {
                var light = child.GetComponent<Light2D>();

                float randomIntensity = Random.Range(0.1f, 1.5f);

                light.intensity = randomIntensity;
            }
        }
    }

    void Update()
    {
        foreach (Transform child in transform)
        {
            child.Rotate(0, 0, 1 * (Time.deltaTime * 2));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(plusX, minusX);
    }
}