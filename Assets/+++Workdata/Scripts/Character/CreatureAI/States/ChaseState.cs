using UnityEngine;

public class ChaseState : State
{
    #region serialized fields
    [Header(nameof(ChaseState))]

    [SerializeField] protected State attackState;
    [SerializeField] protected RoamState roamState;
    [SerializeField] protected float attackDistance = 3;
    #endregion

    #region private fields
    #endregion

    public override State SwitchStateInternal()
    {
        if (creatureLogic.DistanceFromTarget <= attackDistance)
            return attackState;

        if (creatureLogic.TargetStatusManager.gameObject == null)
        {
            creatureLogic.SetTargetStatusManager(null);
            creatureLogic.SetCanSeePlayer(false);
            return roamState;
        }

        if (creatureLogic.DistanceFromTarget >= creatureLogic.DetectionRadius + 5)
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
        if (!creatureLogic.gameObject) return;
        if (!creatureLogic.TargetStatusManager) return;

        creatureLogic.SetDistanceFromTarget(Vector3.Distance(creatureLogic.TargetStatusManager.transform.position, creatureLogic.transform.position));
        creatureLogic.agent.SetDestination(creatureLogic.TargetStatusManager.transform.position);
    }

}
