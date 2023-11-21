using System.Collections.Generic;
using UnityEngine;

public class RoamState : State
{
    #region serialized fields
    [Header(nameof(RoamState))]
    [SerializeField] float minRoamRange = 5f;
    [SerializeField] float maxRoamRange = 5f;
    [SerializeField] List<Health> healthTargets = new();
    [SerializeField] State chaseState;
    #endregion

    #region private fields
    Vector2 roamPosition;
    Vector3 startingPosition;
    #endregion

    public override State SwitchStateInternal()
    {
        if (creatureLogic.TargetHealthScript != null)
            return chaseState;
        else
            return this;
    }

    protected override void EnterInternal()
    {
    }

    protected override void UpdateInternal()
    {
        HandleDetection();
        HandleRoaming();
        creatureLogic.HandleRotate();
    }

    protected override void FixedUpdateInternal()
    {
    }

    protected override void ExitInternal()
    {
    }

    void Start()
    {
        startingPosition = transform.position;
        roamPosition = startingPosition;
    }


    void HandleRoaming()
    {
        creatureLogic.agent.SetDestination(roamPosition);

        if (Vector3.Distance(transform.position, roamPosition) < stateAgentStoppingDistance)
            roamPosition = GetRandomRoamingPosition();

    }

    Vector3 GetRandomRoamingPosition()
    {
        return startingPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(minRoamRange, maxRoamRange);
    }

    public void HandleDetection()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, creatureLogic.DetectionRadius, creatureLogic.TargetLayer);

        if (colliders.Length == 0)
        {
            healthTargets.Clear();
            return;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            Health _healthTarget = colliders[i].GetComponentInChildren<Health>();

            if (!_healthTarget)
                continue;

            if (!healthTargets.Contains(_healthTarget))
                healthTargets.Add(_healthTarget);

            LookLogic(_healthTarget);
        }
    }

    void LookLogic(Health _healthTarget)
    {
        Vector2 targetDirection = (_healthTarget.transform.position - transform.position).normalized;

        if (Vector2.Angle(transform.up, targetDirection) < creatureLogic.DetectionAngle / 2)
        {
            if (!Physics2D.Raycast(transform.position, targetDirection, creatureLogic.DistanceFromTarget, creatureLogic.ObstacleLayer))
            {
                creatureLogic.SetCanSeePlayer(true);
                creatureLogic.SetTargetHealthScript(_healthTarget);
            }
            else
            {
                creatureLogic.SetCanSeePlayer(false);
                creatureLogic.SetTargetHealthScript(null);
            }
        }
        else if (creatureLogic.CanSeeTarget)
            creatureLogic.SetCanSeePlayer(false);
    }
}