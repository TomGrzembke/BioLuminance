using MyBox;
using UnityEngine;

public class TentacleBehavior : MonoBehaviour
{
    #region serialized fields
    [Foldout("TailCustomization", true)]
    [SerializeField] Transform tailEnd;
    [Foldout("TailCustomization", false)]
    [SerializeField] Transform[] bodyParts;

    [SerializeField] Transform targetDir;
    [SerializeField] PointFollowMode pointFollowMode;

    [Tooltip("Will be multiplied times 5 when switching to Point follow mode: stack")]
    [SerializeField] int length;
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

            if (fouldOutOnStart)
                ResetPos();
        }
    }

    void FixedUpdate()
    {
        WiggleLogic();

        AttachLogic();

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
            for (int i = 1; i < segmentPoses.Length; i++)
            {
                segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], GetLastSegmentPose(i) + targetDir.right * GetVertexDistance(), ref segmentV[i], GetSmoothSpeed() + i / trailSpeed);
            }
        }
        else if (pointFollowMode == PointFollowMode.stack)
        {
            for (int i = 1; i < segmentPoses.Length; i++)
            {
                targetPos = GetLastSegmentPose(i) + (segmentPoses[i] - GetLastSegmentPose(i)).normalized * GetVertexDistance();
                segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], GetSmoothSpeed());
                
                if (bodyParts.Length != 0 && bodyParts.Length >= i)
                    bodyParts[i - 1].position = segmentPoses[i];
            }
        }
        lineRend.SetPositions(segmentPoses);
    }

    Vector3 GetLastSegmentPose(int i)
    {
        return segmentPoses[i - 1];
    }

    float GetSmoothSpeed()
    {
        float tempSmothspeed = smoothSpeed / 100;
        if (pointFollowMode == PointFollowMode.overlap)
            return tempSmothspeed;
        else if (pointFollowMode == PointFollowMode.stack)
            return tempSmothspeed / 200;

        return tempSmothspeed;
    }

    float GetVertexDistance()
    {
        return (vertexDistance / 10);
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

    void ResetPos()
    {
        segmentPoses[0] = targetDir.position;
        for (int i = 1; i < length; i++)
        {
            segmentPoses[i] = GetLastSegmentPose(i) + targetDir.right * vertexDistance;
        }
        lineRend.SetPositions(segmentPoses);
    }
}