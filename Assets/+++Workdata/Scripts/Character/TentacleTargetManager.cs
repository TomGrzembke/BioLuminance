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

    void SetTargets()
    {
        RemoveAllNullTargets();

        float tentacleCount = tentacles.Count;
        float targetCount = targetPoints.Count;
        //tentacles[i].SetGrabTarget(attackTarget);
    }

    private void RemoveAllNullTargets()
    {
        for (int i = 0; i < targetPoints.Count; i++)
        {
            if (targetPoints[i] == null)
                targetPoints.RemoveAt(i);
        }
    }

    public void AddAttackPoint(Transform attackTarget)
    {
        targetPoints.Add(attackTarget);

        for (int i = 0; i < tentacles.Count; i++)
        {
            tentacles[i].SetGrabTarget(attackTarget);
        }
    }

    public void AddAttackPoint(List<StatusManager> attackTarget, Transform target = null)
    {
        for (int i = 0; i < attackTarget.Count; i++)
        {
            if (!targetPoints.Contains(attackTarget[i].transform))
            {
                if (!target)
                    targetPoints.Add(attackTarget[i].transform);
                else
                    targetPoints.Add(attackTarget[i].GrabManager.GetClosestGrabTrans(target.position));
            }
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