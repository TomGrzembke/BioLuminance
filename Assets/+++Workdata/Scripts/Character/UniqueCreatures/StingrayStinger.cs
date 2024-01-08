using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingrayStinger : MonoBehaviour
{
    public GameObject snapStingerTo;
    public float stingerRadius;
    public LayerMask creatureLayer;
    [SerializeField] List<StatusManager> statusTargets = new();
    [SerializeField] private List<Collider2D> colliders;

    private void Update()
    {
        HandleDetection();
    }

    public void HandleDetection()
    {
        colliders = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, stingerRadius, creatureLayer));

        for (int i = 0; i < colliders.Count; i++)
        {
            if (!colliders[i].TryGetComponent(out StatusManager statusTarget))
                statusTarget = colliders[i].GetComponentInChildren<StatusManager>();
            
            if (!statusTargets.Contains(statusTarget))
                statusTargets.Add(statusTarget);
        }
    }
}