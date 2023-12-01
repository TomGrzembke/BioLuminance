using System.Collections.Generic;
using UnityEngine;

public class GrabTransformManager : MonoBehaviour
{
    #region serialized fields
    [SerializeField] List<Transform> grabTrans = new();
    #endregion

    #region private fields

    #endregion

    void OnValidate()
    {
        grabTrans.Clear();
        for (int i = 0; i < transform.childCount; i++)
            grabTrans.Add(transform.GetChild(i));
    }

    public Transform GetClosestGrabTrans(Vector3 pos)
    {
        float closestDistance = 100;
        Transform closestTrans = null;

        for (int i = 0; i < grabTrans.Count; i++)
        {
            float distance = Vector3.Distance(grabTrans[i].position, pos);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTrans = grabTrans[i];
            }
        }

        return closestTrans;
    }
}