using MyBox;
using UnityEngine;

public class TentacleBehavior : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Transform targetDir;
    [SerializeField] PointFollowMode pointFollowMode;

    [SerializeField] int length;
    [Tooltip("Distance between created Points, will feel smoother when smaller and more stagnant whem higher")]
    [SerializeField] float vertexDistance;
    [Tooltip("Determines the delay of how fast the points will follow the following point")]
    [SerializeField] float smoothSpeed;
    [ConditionalField(nameof(pointFollowMode), false, PointFollowMode.overlap), SerializeField] float trailSpeed = 350;

    [SerializeField] WiggleMode wiggleMode;
    [ConditionalField(nameof(wiggleMode), false, WiggleMode.wiggle), SerializeField] float wiggleSpeed = 10;
    [ConditionalField(nameof(wiggleMode), false, WiggleMode.wiggle), SerializeField] float wiggleMagnitude = 20;
    [ConditionalField(nameof(wiggleMode), false, WiggleMode.wiggle), SerializeField] Transform wiggleDir;
    #endregion

    #region private fields
    LineRenderer lineRend;
    Vector3[] segmentPoses;
    Vector3[] segmentV;
    Vector3 targetPos;
    #endregion

    #region enums/OnValidate
    enum PointFollowMode
    {
        overlap,
        stack
    }
    enum WiggleMode
    {
        dontWiggle,
        wiggle
    }

    void Awake() => lineRend = GetComponent<LineRenderer>();
    #endregion

    void Start()
    {
        if (pointFollowMode == PointFollowMode.overlap)
        {
            lineRend.positionCount = length;
            segmentPoses = new Vector3[length];
            segmentV = new Vector3[length];
        }
        else if (pointFollowMode == PointFollowMode.stack)
        {
            int multipliedLength = length * 5;
            lineRend.positionCount = multipliedLength;
            segmentPoses = new Vector3[multipliedLength];
            segmentV = new Vector3[multipliedLength];
        }
    }

    void Update()
    {
        WiggleLogic();

        AttachLogic();

        PointFollowUpLogic();
    }

    void PointFollowUpLogic()
    {
        if (pointFollowMode == PointFollowMode.overlap)
        {
            for (int i = 1; i < segmentPoses.Length; i++)
            {
                segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + targetDir.right * vertexDistance, ref segmentV[i], smoothSpeed + i / trailSpeed);
            }
        }
        else if (pointFollowMode == PointFollowMode.stack)
        {
            for (int i = 1; i < segmentPoses.Length; i++)
            {
                targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * vertexDistance;
                segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], smoothSpeed / 200);
            }
        }
        lineRend.SetPositions(segmentPoses);
    }

    void AttachLogic()
    {
        segmentPoses[0] = targetDir.position;
    }

    void WiggleLogic()
    {
        if (wiggleDir != null && wiggleMode == WiggleMode.wiggle)
            wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);
    }
}