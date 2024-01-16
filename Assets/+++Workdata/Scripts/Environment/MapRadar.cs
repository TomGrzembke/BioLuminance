using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRadar : MonoBehaviour
{
    [SerializeField] float growthSpeed = 40f;
    [SerializeField] float growthSize = 200f;
    private void Update()
    {
        gameObject.transform.localScale += new Vector3(1f,1f) * (Time.deltaTime * growthSpeed);

        if (gameObject.transform.localScale.x > growthSize)
            gameObject.transform.localScale = Vector3.zero;
    }
}