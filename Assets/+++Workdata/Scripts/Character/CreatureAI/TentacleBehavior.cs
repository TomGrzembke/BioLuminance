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
    [ConditionalField(nameof(grabTrans)), SerializeField] int grabSpeed = 60;

    [Foldout("TailCustomization", false)]

    [SerializeField] Transform grabTrans;
    public Transform GrabTrans => grabTrans;

    [SerializeField, ConditionalField(true, nameof(GetIsInPlaymode))] bool fouldOutOnStart = true;
    bool GetIsInPlaymode() => !Application.isPlaying;

    [SerializeField] Transform attachTrans;
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

    LineRenderer lineRend;
    Vector3[] segmentPoses;
    Vector3[] segmentV;
    Vector3 targetPos;
    Transform defaultGrabTrans;
    public Transform DefaultGrabTrans => defaultGrabTrans;
    #endregion
    void Awake() => lineRend = GetComponent<LineRenderer>();

    void Start()
    {
        defaultGrabTrans = grabTrans;
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
                segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], (GetLastSegmentPose(i) + attachTrans.right * calc_vertexDistance) + GetCalcGrabPos(i), ref segmentV[i], calc_smoothSpeed + i / trailSpeed);

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
        if (grabTrans == null)
            return Vector3.zero;

        if (pointFollowMode == PointFollowMode.stack)
            return (grabTrans.position - GetLastSegmentPose(i)).normalized / grabSpeed;
        else if (pointFollowMode == PointFollowMode.overlap)
            return (grabTrans.position - GetLastSegmentPose(i)) / 10;

        else
            return Vector3.zero;
    }

    void MoveBodyParts(int i)
    {
        if (bodyParts.Length == 0) return;
        if (bodyParts.Length < i) return;
        if (bodyParts[i - 1] == null) return;

        print(bodyParts[i - 1].name);
        bodyParts[i - 1].position = segmentPoses[i];
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
        segmentPoses[0] = attachTrans.position;
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
            segmentPoses[i] = GetLastSegmentPose(i) + attachTrans.right;
        }
        lineRend.SetPositions(segmentPoses);
    }

    public void SetGrabTarget(Transform grabTarget)
    {
        if (grabTarget == null)
            grabTrans = defaultGrabTrans;
        else
            grabTrans = grabTarget;
    }
}