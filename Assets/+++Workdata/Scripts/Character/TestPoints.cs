using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class TestPoints : MonoBehaviour
{
    private PointSystem pointSystem;
    public Creatures creatures;
    
    [ButtonMethod]
    public void PercentOption()
    {
        pointSystem.CalculatePoints(creatures);
    }
}