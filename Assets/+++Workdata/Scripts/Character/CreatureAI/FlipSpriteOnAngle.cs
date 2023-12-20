using MyBox;
using UnityEngine;

/// <summary> Gets the sr and flips the flipY when angle of target is reached</summary>
public class FlipSpriteOnAngle : MonoBehaviour
{
    enum Flip
    {
        x,
        y
    }

    #region serialized fields
    [SerializeField] Transform rotationParent;
    [MinMaxRange(0, 360)]
    [SerializeField] RangedFloat flipRange = new(0, 180);
    [SerializeField] Flip flip = Flip.y;
    [SerializeField] bool invert;
    #endregion

    #region private fields
    SpriteRenderer sr;
    [SerializeField] bool flipped;
    #endregion

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        float angle = rotationParent.rotation.eulerAngles.z;

        if (flipped != (angle > flipRange.Min && angle < flipRange.Max))
        {
            print("a");
            transform.localScale = new(1, flipped ? -1 : 1, 1);
            flipped = !flipped;
        }

        return;
        if (rotationParent == null) return;


        if (flip == Flip.x)
        {
            sr.flipX = angle > flipRange.Min && angle < flipRange.Max;
            if (invert)
                sr.flipX = !sr.flipX;
        }
        else if (flip == Flip.y)
        {
            sr.flipY = angle > flipRange.Min && angle < flipRange.Max;
            if (invert)
                sr.flipY = !sr.flipY;
        }


    }
}