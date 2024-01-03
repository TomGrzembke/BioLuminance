using MyBox;
using UnityEngine;

/// <summary> Flips the object with context</summary>
public class FlipSpriteOnAngle : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Transform rotationParent;
    [MinMaxRange(0, 360)]
    [SerializeField] RangedFloat flipRange = new(0, 180);
    float angle;
    #endregion

    #region private fields
    bool flipped;
    bool inFlipRange;
    #endregion

    void Update()
    {
        FlipLogic();
    }

    void FlipLogic()
    {
        angle = rotationParent.rotation.eulerAngles.z;
        inFlipRange = angle > flipRange.Min && angle < flipRange.Max;

        if (flipped != inFlipRange)
        {
            Vector3 localScale = transform.localScale;
            localScale.y = -localScale.y;
            transform.localScale = localScale;

            flipped = inFlipRange;
        }
    }
}