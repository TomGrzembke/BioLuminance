using UnityEngine;

public class TentacleBehavior : MonoBehaviour
{
    #region serialized fields
    [SerializeField] int length;
    [SerializeField] LineRenderer lineRend;
    [SerializeField] Vector3[] segmentPoses;
    [SerializeField] Transform targetDir;
    [SerializeField] float targetDist;
    [SerializeField] float smoothSpeed;
    [SerializeField] float trailSpeed = 350;
    #endregion

    #region private fields
    Vector3[] segmentV;
    #endregion

    void Start()
    {
        lineRend.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
    }

    void Update()
    {
        segmentPoses[0] = targetDir.position;

        for (int i = 1; i < segmentPoses.Length ; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + targetDir.right * targetDist, ref segmentV[i], smoothSpeed + i / trailSpeed);
        }
        lineRend.SetPositions(segmentPoses);
    }
}