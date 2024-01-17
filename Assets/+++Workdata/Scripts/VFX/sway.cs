using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sway : MonoBehaviour
{
    public float rotationSpeed = 1f; // Speed variable used to control the animation
    public float rotationOffset = 50f; // Rotate by 50 units
    public float finalAngle;
    
    public Godrays godrays;
    public Vector3 startAngle; // Reference to the object's original angle values

    private void Start()
    {
        godrays = GetComponentInParent<Godrays>();
        startAngle = transform.eulerAngles;
    }

    private void Update()
    {
        finalAngle = startAngle.y + Mathf.Sin(Time.time * rotationSpeed) * rotationOffset; // Calculate animation angle

        transform.eulerAngles = new Vector3(0, startAngle.y, 180 + finalAngle / 10); // Apply the new angle to the object
    }
}