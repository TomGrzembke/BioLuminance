using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class TestPoints : MonoBehaviour
{
    private PointSystem pointSystem;
    public Creatures creatures;
    public float points;
    public float percentage;

    private void OnValidate()
    {
        pointSystem = GetComponent<PointSystem>();
    }

    [ButtonMethod]
    public void Points()
    {
        pointSystem.SetCreatureDnaStats(creatures, points, percentage);
    }

    [ButtonMethod]
    public void PercentOption1()
    {
        pointSystem.CalculatePoints(creatures);
    }

    [ButtonMethod]
    public void PercentOption2()
    {
        pointSystem.CalculatePoints2(creatures);
    }
}