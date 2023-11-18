using UnityEngine;

public class ChaseState : State
{
    #region serialized fields
    [Header(nameof(ChaseState))]

    [SerializeField] protected float chaseRange;
    [SerializeField] protected float attackRange = 2;
    [SerializeField] protected float acceleration = 5;
    [SerializeField] protected float oldAcceleration = 5;
    [SerializeField] protected float chaseSpeed = 2;

    [SerializeField] protected AttackState attackState;
    [SerializeField] protected RoamState roamState;
    [SerializeField] AgressiveChaseState agressiveChaseState;
    #endregion

    #region private fields
    #endregion

    public override State SwitchStateInternal()
    {
        if (TimeInState >= 5f)
            return agressiveChaseState;

        else if (creatureLogic.DistanceFromTarget <= attackRange)
            return attackState;

        else if (creatureLogic.DistanceFromTarget >= chaseRange)
        {
            creatureLogic.SetTargetHealthScript(null);
            creatureLogic.SetCanSeePlayer(false);
            return roamState;
        }

        return this;
    }

    protected override void EnterInternal()
    {
        oldAcceleration = creatureLogic.AgentAcceleration;
        creatureLogic.RefreshAgentVars(chaseSpeed, acceleration, attackRange);
    }

    protected override void UpdateInternal()
    {
        HandleMovement();
        creatureLogic.HandleRotate();
    }

    protected override void FixedUpdateInternal()
    {
    }

    protected override void ExitInternal()
    {
        creatureLogic.RefreshAgentVars(creatureLogic.AgentSpeed, oldAcceleration, creatureLogic.AgentStoppingDistance);
    }

    void HandleMovement()
    {
        creatureLogic.SetDistanceFromTarget(Vector3.Distance(creatureLogic.TargetHealthScript.transform.position, creatureLogic.transform.position));
        creatureLogic.agent.SetDestination(creatureLogic.TargetHealthScript.transform.position);
    }

}
