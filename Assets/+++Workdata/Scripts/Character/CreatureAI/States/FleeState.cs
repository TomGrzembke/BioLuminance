using MyBox;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
    #region serialized fields
    [MinMaxRange(0, 10)]
    [SerializeField] RangedFloat randomMoveLength = new(0, 10);
    [SerializeField] List<StatusManager> statusTargets = new();
    [SerializeField] HealthSubject nearestHealthSubj;
    #endregion

    #region private fields

    #endregion

    public override State SwitchStateInternal()
    {
        return this;
    }

    protected override void EnterInternal()
    {
    }

    protected override void ExitInternal()
    {
    }

    protected override void FixedUpdateInternal()
    {
        creatureLogic.HandleRotate();
    }

    protected override void UpdateInternal()
    {
        HandleDetection();
        if (creatureLogic.agent.hasPath) return;

        int pathHorizontal = Random.Range(-1, 2);
        int pathVertical = Random.Range(-1, 2);
        Vector3 pathAddVec3 = new(pathVertical, pathHorizontal);

        float randomMultiplier = Random.Range(randomMoveLength.Min, randomMoveLength.Max);
        creatureLogic.agent.SetDestination(transform.position + pathAddVec3 * randomMultiplier);
    }

    public void HandleDetection()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, creatureLogic.DetectionRadius, creatureLogic.CreatureLayer);

        if (colliders.Length == 0)
        {
            statusTargets.Clear();
            return;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            StatusManager statusManager = colliders[i].GetComponentInChildren<StatusManager>();

            if (!statusManager)
                continue;

            if (!statusTargets.Contains(statusManager))
                statusTargets.Add(statusManager);

            var dangerDistance = statusManager.transform.position - transform.position;
        }
    }
}