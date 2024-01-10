using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class TestPoints : MonoBehaviour
{
    public PointSystem pointSystem;
    public Creatures creatures;

    private void Awake()
    {
        pointSystem = FindObjectOfType<PointSystem>();
    }

    [ButtonMethod]
    public void PercentOption()
    {
        pointSystem.CalculatePoints(creatures);
    }
}