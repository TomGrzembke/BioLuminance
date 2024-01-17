using UnityEngine;

public class StingrayAttack : State
{
    #region serialized fields

    [SerializeField] RoamState roamState;
    [SerializeField] State chaseState;
    #endregion

    #region private fields
    #endregion

    public override State SwitchStateInternal()
    {
        if (creatureLogic.TargetStatusManager == null)
            return roamState;

        if (creatureLogic.DistanceFromTarget >= creatureLogic.DetectionRadius + 30)
        {
            return chaseState;
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
        if (!creatureLogic.agent.hasPath)
            creatureLogic.agent.SetDestination(creatureLogic.TargetStatusManager.transform.position);
    }
}