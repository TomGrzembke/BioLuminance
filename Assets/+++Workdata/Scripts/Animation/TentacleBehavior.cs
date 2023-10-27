using MyBox;
using UnityEngine;

public class TentacleBehavior : MonoBehaviour
{

    #region serialized fields
    [Foldout("TailCustomization", true)]
    [SerializeField] Transform tailEnd;
    [SerializeField] Transform[] bodyParts;

    [Foldout("TailCustomization", false)]

    [SerializeField] Transform target;
    [SerializeField] Transform targetDir;
    [SerializeField] PointFollowMode pointFollowMode;

    [Tooltip("Will be multiplied times 5 when switching to Point follow mode: stack")]
    [SerializeField] int length = 30;
    [Tooltip("Distance between created Points, will feel smoother when smaller and more stagnant when higher")]
    [SerializeField] float vertexDistance;
    [Tooltip("Determines the delay of how fast the points will follow the following point")]
    [SerializeField] float smoothSpeed;
    [ConditionalField(nameof(pointFollowMode), false, PointFollowMode.overlap), SerializeField] float trailSpeed = 350;
    [ConditionalField(nameof(pointFollowMode), false, PointFollowMode.stack), SerializeField] bool fouldOutOnStart = true;

    [SerializeField] WiggleMode wiggleMode;
    [ConditionalField(nameof(wiggleMode), false, WiggleMode.wiggle), SerializeField] float wiggleSpeed = 10;
    [ConditionalField(nameof(wiggleMode), false, WiggleMode.wiggle), SerializeField] float wiggleMagnitude = 20;
    [ConditionalField(nameof(wiggleMode), false, WiggleMode.wiggle), SerializeField] Transform wiggleDir;
    #endregion

    #region private fields
    float calc_vertexDistance;
    float calc_smoothSpeed;
    /// <summary> Used for Stack length</summary>
    int calc_length;
    int calc_bodyPartDistance = 1;

    LineRenderer lineRend;
    Vector3[] segmentPoses;
    Vector3[] segmentV;
    Vector3 targetPos;
    #endregion

    #region enums
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
        Recalculate();
        StartSettings();
    }
    void OnValidate()
    {
        Recalculate();
    }

    #region recalculate/normalize values
    void Recalculate()
    {
        calc_vertexDistance = vertexDistance / 10;

        if (pointFollowMode == PointFollowMode.overlap)
        {
            calc_smoothSpeed = smoothSpeed / 100;
            calc_length = length;
        }
        else if (pointFollowMode == PointFollowMode.stack)
        {
            calc_smoothSpeed = smoothSpeed / 200;
            calc_bodyPartDistance = 10;
            calc_length = length * 12;
        }
    }

    #endregion

    void StartSettings()
    {
        if (pointFollowMode == PointFollowMode.overlap)
        {
            lineRend.positionCount = calc_length;
            segmentPoses = new Vector3[calc_length];
            segmentV = new Vector3[calc_length];
        }
        else if (pointFollowMode == PointFollowMode.stack)
        {
            lineRend.positionCount = calc_length;
            segmentPoses = new Vector3[calc_length];
            segmentV = new Vector3[calc_length];

            if (fouldOutOnStart)
                FoldoutOnStart();
        }
    }

    void FixedUpdate()
    {
        WiggleLogic();

        AttachedPart();

        PointFollowUpLogic();

        TailEnd();
    }

    void TailEnd()
    {
        if (tailEnd != null)
            tailEnd.position = segmentPoses[segmentPoses.Length - 1];
    }

    void PointFollowUpLogic()
    {
        if (pointFollowMode == PointFollowMode.overlap)
        {
            for (int i = 1; i < calc_length; i++)
            {
                segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], GetLastSegmentPose(i) + targetDir.right * calc_vertexDistance, ref segmentV[i], calc_smoothSpeed + i / trailSpeed);
                MoveBodyParts(i);
            }
        }
        else if (pointFollowMode == PointFollowMode.stack)
        {
            for (int i = 1; i < calc_length; i++)
            {
                targetPos = GetLastSegmentPose(i) + (segmentPoses[i] - GetLastSegmentPose(i)).normalized * calc_vertexDistance;
                segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], calc_smoothSpeed);

                MoveBodyParts(i);
            }
        }
        lineRend.SetPositions(segmentPoses);
    }

    void MoveBodyParts(int i)
    {
        if (bodyParts.Length == 0)
            return;
        if (bodyParts.Length < i)
            return;
        if (!(calc_length > i + calc_bodyPartDistance))
            return;

        bodyParts[i - 1].position = segmentPoses[i * calc_bodyPartDistance];
    }

    Vector3 GetLastSegmentPose(int i)
    {
        return segmentPoses[i - 1];
    }

    Vector3 GetNextSegmentPose(int i)
    {
        return segmentPoses[i + 1];
    }

    private void AttachedPart()
    {
        segmentPoses[0] = targetDir.position;
    }

    void WiggleLogic()
    {
        if (wiggleDir != null && wiggleMode == WiggleMode.wiggle)
            wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);
    }

    void FoldoutOnStart()
    {
        AttachedPart();
        for (int i = 1; i < calc_length; i++)
        {
            segmentPoses[i] = GetLastSegmentPose(i) + targetDir.right;
        }
        lineRend.SetPositions(segmentPoses);
    }
}