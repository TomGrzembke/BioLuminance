using MyBox;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TentacleBehavior : MonoBehaviour
{
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
    #endregion

    #region serialized fields
    [Foldout("TailCustomization", true)]
    [SerializeField] Transform tailEnd;
    [SerializeField] Transform[] bodyParts;

    [Foldout("TailCustomization", false)]

    [SerializeField] Transform grabPos;

    [SerializeField, ConditionalField(true, nameof(GetIsInPlaymode))] bool fouldOutOnStart = true;
    bool GetIsInPlaymode() => !Application.isPlaying;

    [SerializeField] Transform attachPos;
    [SerializeField] PointFollowMode pointFollowMode;

    [Tooltip("Will be multiplied times 5 when switching to Point follow mode: stack")]
    [SerializeField] int length = 30;
    [Tooltip("Distance between created Points, will feel smoother when smaller and more stagnant when higher")]
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
    void Awake() => lineRend = GetComponent<LineRenderer>();

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
        segmentV = new Vector3[calc_length];
        lineRend.positionCount = calc_length;
        segmentPoses = new Vector3[calc_length];

        if (fouldOutOnStart)
            FoldoutOnStart();
    }

    void Update()
    {
        WiggleLogic();

        AttachedPart();

        PointFollowUpLogic();

        TailEnd();
    }

    void TailEnd()
    {
        if (tailEnd == null)
            return;

        tailEnd.position = segmentPoses[segmentPoses.Length - 1];
    }

    void PointFollowUpLogic()
    {
        if (pointFollowMode == PointFollowMode.overlap)
        {
            for (int i = 1; i < calc_length; i++)
            {
                segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], (GetLastSegmentPose(i) + attachPos.right * calc_vertexDistance) + GetCalcGrabPos(i), ref segmentV[i], calc_smoothSpeed + i / trailSpeed);

                MoveBodyParts(i);
            }
        }
        else if (pointFollowMode == PointFollowMode.stack)
        {
            for (int i = 1; i < calc_length; i++)
            {
                targetPos = GetLastSegmentPose(i) + (segmentPoses[i] - GetLastSegmentPose(i)).normalized * calc_vertexDistance;

                segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos + GetCalcGrabPos(i), ref segmentV[i], calc_smoothSpeed);

                MoveBodyParts(i);
            }
        }

        lineRend.SetPositions(segmentPoses);
    }

    Vector3 GetCalcGrabPos(int i)
    {
        if (grabPos == null)
            return Vector3.zero;

        if (pointFollowMode == PointFollowMode.stack)
            return (grabPos.position - GetLastSegmentPose(i)).normalized / 100;
        else if (pointFollowMode == PointFollowMode.overlap)
            return (grabPos.position - GetLastSegmentPose(i)).normalized / 10;

        else
            return Vector3.zero;
    }

    void MoveBodyParts(int i)
    {
        if (bodyParts.Length == 0)
            return;
        if (bodyParts.Length < i)
            return;
        if (calc_length < i + calc_bodyPartDistance)
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
        segmentPoses[0] = attachPos.position;
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
            segmentPoses[i] = GetLastSegmentPose(i) + attachPos.right;
        }
        lineRend.SetPositions(segmentPoses);
    }
}