using UnityEngine;

public class DepthTracker : MonoBehaviour
{
    #region serialized fields
    [SerializeField] SpriteRenderer mapSprite;
    [SerializeField] Transform subject;

    public float SubjectVerticalPos => subjectVerticalPos;
    [ShowOnly, SerializeField] float subjectVerticalPos;
    #endregion

    #region private fields
    float maxHeight;
    float minHeight;
    #endregion

    void Awake()
    {
        maxHeight = mapSprite.bounds.max.y;
        minHeight = mapSprite.bounds.min.y;
    }

    void Update()
    {
        subjectVerticalPos = Mathf.Clamp(subject.position.y, minHeight, maxHeight);
    }
}