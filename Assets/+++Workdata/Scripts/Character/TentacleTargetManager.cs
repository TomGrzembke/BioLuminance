using MyBox;
using System.Collections.Generic;
using UnityEngine;

public class TentacleTargetManager : MonoBehaviour
{
    #region serialized fields
    [SerializeField] List<TentacleBehavior> tentacles;
    [SerializeField] List<LimbSubject> targetLimbs;
    [SerializeField] List<Transform> targetTrans;
    #endregion

    #region private fields

    #endregion

    public void SetTargets()
    {
        RemoveAllNullTargets();
        targetTrans.Clear();

        int tentacleCount = tentacles.Count;
        int targetCount = targetLimbs.Count;

        switch (targetCount)
        {
            case 0:
                targetTrans.Clear();
                ResetTentacles();
                break;
            case 1:
                for (int i = 0; i < tentacleCount; i++)
                {
                    AddTargetTrans(targetLimbs[0].ownStatusManager.GrabManager.GetRandomGrabTrans());
                }
                break;
            default:
                for (int i = 0; i < tentacleCount; i++)
                {
                    AddTargetTrans(targetLimbs[Random.Range(0, targetCount)].ownStatusManager.GrabManager.GetRandomGrabTrans());
                }
                break;
        }

        targetTrans.Shuffle();

        for (int i = 0; i < tentacleCount; i++)
        {
            if (i > targetTrans.Count - 1)
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
        for (int i = 0; i < targetLimbs.Count; i++)
        {
            if (targetLimbs[i] == null)
            {
                targetLimbs.RemoveAt(i);
                i--;
            }
        }
    }

    public void AddAttackStatusManager(LimbSubject attackTarget)
    {
        targetLimbs.Clear();
        targetLimbs.Add(attackTarget);
        SetTargets();
    }

    public void SetAttackStatusManager(List<LimbSubject> attackTarget)
    {
        targetLimbs.Clear();
        for (int i = 0; i < attackTarget.Count; i++)
        {
            if (!targetLimbs.Contains(attackTarget[i]))
            {
                targetLimbs.Add(attackTarget[i]);
            }
        }

        SetTargets();
    }

    public void RemoveAttackStatusManager(LimbSubject attackTarget)
    {
        if (targetLimbs.Remove(attackTarget))
            SetTargets();
    }

    public void SetOneTarget(Transform target)
    {
        targetLimbs.Clear();
        targetTrans.Clear();
        ResetTentacles(target);
    }

    public void AddTentacle(GameObject newTentacle)
    {

    }

    public void SetTentacles(bool condition)
    {
        for (int i = 0; i < tentacles.Count; i++)
        {
            tentacles[i].TryGetComponent(out TentacleDetection _tentacleDetection);
            if(_tentacleDetection)
                _tentacleDetection.enabled = condition;
        }
    }
}