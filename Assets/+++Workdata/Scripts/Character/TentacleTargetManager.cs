using System.Collections.Generic;
using UnityEngine;

public class TentacleTargetManager : MonoBehaviour
{
    #region serialized fields
    [SerializeField] List<TentacleBehavior> tentacles;
    [SerializeField] List<Transform> targetPoints;
    #endregion

    #region private fields

    #endregion

    public void AddAttackPoint(Transform attackTarget)
    {
        targetPoints.Add(attackTarget);

        for (int i = 0; i < tentacles.Count; ++i)
        {
            tentacles[i].SetGrabTarget(attackTarget);
        }
    }

    public void RemoveAttackPoint(Transform attackTarget)
    {
        if (targetPoints.Remove(attackTarget))
            for (int i = 0; i < tentacles.Count; ++i)
            {
                tentacles[i].SetGrabTarget(null);
            }
    }
}