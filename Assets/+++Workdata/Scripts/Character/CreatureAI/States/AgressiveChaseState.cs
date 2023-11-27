using UnityEngine;

public class AgressiveChaseState : ChaseState
{
    #region serialized fields

    [SerializeField] float stateDuration = 5;
    #endregion

    #region private fields
    #endregion

    public override State SwitchStateInternal()
    {
        if (TimeInState >= stateDuration)
        {
            return roamState;
        }
        if (creatureLogic.DistanceFromTarget <= stateAgentStoppingDistance)
        {
            return attackState;
        }
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

    protected override void ExitInternal()
    {
    }

}