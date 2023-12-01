using MyBox;
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

        for (int i = 0; i < tentacles.Count; i++)
        {
            tentacles[i].SetGrabTarget(attackTarget);
        }
    }

    public void AddAttackPoint(List<StatusManager> attackTarget)
    {
        for (int i = 0; i < attackTarget.Count; i++)
        {
            if (!targetPoints.Contains(attackTarget[i].transform))
                targetPoints.Add(attackTarget[i].transform);
        }

        for (int i = 0; i < tentacles.Count; i++)
        {
            if (attackTarget.Count < i + 1) break;
            tentacles[i].SetGrabTarget(attackTarget[i].transform);
        }

        targetPoints.Shuffle();
    }

    public void RemoveAttackPoint(Transform attackTarget)
    {
        if (targetPoints.Remove(attackTarget))
            for (int i = 0; i < tentacles.Count; ++i)
            {
                tentacles[i].SetGrabTarget(null);
            }
    }

    public void ResetAttackPoint()
    {
        targetPoints.Clear();
    }
}