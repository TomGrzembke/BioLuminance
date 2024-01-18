using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

public class InformationfieldPivot : MonoBehaviour
{
    public RectTransform rectTransform;
    public RectTransform border;
    public Canvas canvas;
    public RectTransform canvasRect;
    public float height;
    public float width;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        canvasRect = canvas.GetComponent<RectTransform>();
        height = canvasRect.rect.height;
        width = canvasRect.rect.width;
    }

    private void Update()
    {
        if (border.position.x > width / 2 || border.position.y > height / 2)
        {
            print("a");
            rectTransform.pivot = new Vector2(1, 1);
        }
        else if (border.position.x < width / 2 || border.position.y < height / 2)
        {
            print("b");
            rectTransform.pivot = new Vector2(0, 0);
        }
    }
}