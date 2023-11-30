using MyBox;
using UnityEngine;

public class FlipObjectOnAngle : MonoBehaviour
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

    #endregion

    void Update()
    {
        float angle = rotationParent.rotation.eulerAngles.z;
        print (angle);
        print(angle > flipRange.Min && angle < flipRange.Max);
        if (angle < flipRange.Min && angle > flipRange.Max)
            transform.Rotate(new Vector3(transform.localRotation.x,180,transform.localRotation.z));
        else
            transform.Rotate(new Vector3(transform.localRotation.x,0,transform.localRotation.z));
            //transform.localRotation = Quaternion.Euler(transform.localRotation.x, 0, transform.localRotation.z);
    }

}