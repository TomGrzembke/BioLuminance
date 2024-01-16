using UnityEngine;

public class StingrayAttack : State
{
    #region serialized fields
    [Header(nameof(ChaseState))]

    [SerializeField] RoamState roamState;
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
        creatureLogic.SetDistanceFromTarget(Vector3.Distance(creatureLogic.TargetStatusManager.transform.position, creatureLogic.transform.position) * 2);
        if (!creatureLogic.agent.hasPath)
            creatureLogic.agent.SetDestination(creatureLogic.TargetStatusManager.transform.position);
    }
}