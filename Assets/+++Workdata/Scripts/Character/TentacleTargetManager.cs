using MyBox;
using System.Collections.Generic;
using UnityEngine;

public class TentacleTargetManager : MonoBehaviour
{
    #region serialized fields
    [SerializeField] List<TentacleBehavior> tentacles;
    [SerializeField] List<StatusManager> targetSM;
    [SerializeField] List<Transform> targetTrans;
    #endregion

    #region private fields

    #endregion

    public void SetTargets()
    {
        RemoveAllNullTargets();
        targetTrans.Clear();

        int tentacleCount = tentacles.Count;
        int targetCount = targetSM.Count;

        switch (targetCount)
        {
            case 0:
                targetTrans.Clear();
                ResetTentacles();
                break;
            case 1:
                for (int i = 0; i < tentacleCount; i++)
                {
                    AddTargetTrans(targetSM[0].GrabManager.GetRandomGrabTrans());
                }
                break;
            default:
                for (int i = 0; i < tentacleCount; i++)
                {
                    AddTargetTrans(targetSM[Random.Range(0, targetCount)].GrabManager.GetRandomGrabTrans());
                }
                break;
        }

        targetTrans.Shuffle();

        for (int i = 0; i < tentacleCount; i++)
        {
            if (i > targetTrans.Count -1)
                break;
            tentacles[i].SetGrabTarget(targetTrans[i].transform);
        }
    }

    public void ResetTentacles(Transform newTarget = null)
    {
        for (int i = 0; i < tentacles.Count; i++)
        {
            tentacles[i].SetGrabTarget(newTarget);
        }
    }

    void AddTargetTrans(Transform singleTarget)
    {
        targetTrans.Add(singleTarget);
    }

    void RemoveAllNullTargets()
    {
        for (int i = 0; i < targetSM.Count; i++)
        {
            if (targetSM[i] == null)
            {
                targetSM.RemoveAt(i);
                i--;
            }
        }
    }

    public void AddAttackStatusManager(StatusManager attackTarget)
    {
        targetSM.Clear();
        targetSM.Add(attackTarget);
        SetTargets();
    }

    public void SetAttackStatusManager(List<StatusManager> attackTarget)
    {
        targetSM.Clear();
        for (int i = 0; i < attackTarget.Count; i++)
        {
            if (!targetSM.Contains(attackTarget[i]))
            {
                targetSM.Add(attackTarget[i]);
            }
        }

        SetTargets();
    }

    public void RemoveAttackStatusManager(StatusManager attackTarget)
    {
        if (targetSM.Remove(attackTarget))
            SetTargets();
    }

    public void SetOneTarget(Transform target)
    {
        targetSM.Clear();
        targetTrans.Clear();
        ResetTentacles(target);
    }
}