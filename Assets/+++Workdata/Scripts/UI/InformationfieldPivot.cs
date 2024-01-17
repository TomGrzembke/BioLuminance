using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class InformationfieldPivot : MonoBehaviour
{
    public RectTransform rectTransform;
    public RectTransform border;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (border.position.x > 980 || border.position.y > 540)
            rectTransform.pivot = new Vector2(1, 1);
        else if (border.position.x < 980 || border.position.y < 540)
            rectTransform.pivot = new Vector2(0, 0);
    }
}