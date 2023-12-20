using MyBox;
using UnityEngine;

/// <summary> Flips the object with context</summary>
public class FlipSpriteOnAngle : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Transform rotationParent;
    [MinMaxRange(0, 360)]
    [SerializeField] RangedFloat flipRange = new(0, 180);
    #endregion

    #region private fields
    bool flipped;
    #endregion

    void Update()
    {
        float angle = rotationParent.rotation.eulerAngles.z;

        if (flipped != (angle > flipRange.Min && angle < flipRange.Max))
        {
            transform.localScale = new(transform.localScale.x, flipped ? -transform.localScale.y : transform.localScale.y, transform.localScale.z);
            flipped = !flipped;
        }
    }
}