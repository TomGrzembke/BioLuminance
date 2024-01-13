using System.ComponentModel;
using UnityEngine;

public class DepthTracker : MonoBehaviour
{
    [SerializeField] SpriteRenderer mapSprite;
    [SerializeField] Transform subject;

    [ShowOnly, SerializeField] float subjectVerticalPosition;

    float maxHeight;
    float minHeight;

    void Awake()
    {
        maxHeight = mapSprite.bounds.max.y;
        minHeight = mapSprite.bounds.min.y;
    }

    void Update()
    {
        subjectVerticalPosition = Mathf.Clamp(subject.position.y, minHeight, maxHeight);
    }
}