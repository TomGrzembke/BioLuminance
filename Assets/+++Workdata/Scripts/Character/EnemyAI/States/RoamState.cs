using System.Collections.Generic;
using UnityEngine;

public class RoamState : State
{
    #region serialized fields
    [Header(nameof(State))]
    [SerializeField] float minRoamRange = 5f;
    [SerializeField] float maxRoamRange = 5f;
    [SerializeField] ChaseState chaseState;
    [SerializeField] float reachedPositionDistance = 1;
    [SerializeField] List<Health> healthTargets = new();
    #endregion

    #region private fields
    Vector2 roamPosition;
    Vector3 startingPosition;
    float oldStoppingDistance;
    #endregion

    public override State SwitchState()
    {
        if (creatureLogic.TargetHealthScript != null)
        {
            return chaseState;
        }
        else
        {
            return this;
        }
    }

    protected override void EnterInternal()
    {
        oldStoppingDistance = creatureLogic.AgentStoppingDistance;
        creatureLogic.RefreshAgentVars(creatureLogic.AgentSpeed, creatureLogic.AgentAcceleration, 0);
    }

    protected override void UpdateInternal()
    {
        HandleDetection();
        HandleRoaming();
        HandleRotate();
    }

    protected override void FixedUpdateInternal()
    {
    }

    protected override void ExitInternal()
    {
        creatureLogic.RefreshAgentVars(creatureLogic.AgentSpeed, creatureLogic.AgentAcceleration, oldStoppingDistance);
    }

    void Start()
    {
        startingPosition = transform.position;
        roamPosition = startingPosition;
    }

    Vector3 GetRandomRoamingPosition()
    {
        return startingPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(minRoamRange, maxRoamRange);
    }

    private void HandleRoaming()
    {
        creatureLogic.agent.SetDestination(roamPosition);

        if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
        {
            roamPosition = GetRandomRoamingPosition();
        }
    }
    private void HandleRotate()
    {
        Vector3 velocity = creatureLogic.agent.velocity;
        velocity.z = 0;
        velocity.Normalize();

        if (velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, velocity);

            float step = creatureLogic.AgentAcceleration * Time.deltaTime;
            creatureLogic.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        }
    }

    public void HandleDetection()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, creatureLogic.DetectionRadius, creatureLogic.DetectionLayer);
        if (colliders.Length == 0)
        {
            healthTargets.Clear();
            return;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            print(colliders[i].name);
            Health _healthTarget = colliders[i].GetComponentInChildren<Health>();

            if (_healthTarget == null)
                continue;

            if (!healthTargets.Contains(_healthTarget))
                healthTargets.Add(_healthTarget);

            Vector2 targetDirection = (_healthTarget.transform.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, targetDirection) < creatureLogic.Angle / 2)
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
            else if (creatureLogic.CanSeePlayer)
                creatureLogic.SetCanSeePlayer(false);
        }
    }
}