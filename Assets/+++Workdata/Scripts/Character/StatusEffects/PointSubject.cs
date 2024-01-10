using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class PointSubject : MonoBehaviour
{
    public PointSystem pointSystem;
    public StatusManager statusManager;
    public Creatures creatures;
    public float points;

    private void Start()
    {
        pointSystem = FindObjectOfType<PointSystem>();
        statusManager = GetComponentInParent<StatusManager>();

        creatures = statusManager.CreatureType;
        Points();
    }
    
    public void Points()
    {
        pointSystem.SetCreatureDnaStats(creatures, points);
    }
    
    [ButtonMethod]
    public void PercentOption()
    {
        pointSystem.CalculatePoints(creatures);
    }
}