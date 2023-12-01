using UnityEngine;

public class ChaseState : State
{
    #region serialized fields
    [Header(nameof(ChaseState))]

    [SerializeField] protected float chaseRange;
    [SerializeField] protected AttackState attackState;
    [SerializeField] protected RoamState roamState;
    #endregion

    #region private fields
    #endregion

    public override State SwitchStateInternal()
    {
        if (creatureLogic.DistanceFromTarget <= stateAgentStoppingDistance)
            return attackState;

        else if (creatureLogic.DistanceFromTarget >= chaseRange)
        {
            creatureLogic.SetTargetStatusManager(null);
            creatureLogic.SetCanSeePlayer(false);
            return roamState;
        }

        return this;
    }

    protected override void EnterInternal()
    {
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
    }

    void HandleMovement()
    {
        creatureLogic.SetDistanceFromTarget(Vector3.Distance(creatureLogic.TargetStatusManager.transform.position, creatureLogic.transform.position));
        creatureLogic.agent.SetDestination(creatureLogic.TargetStatusManager.transform.position);
    }

}
