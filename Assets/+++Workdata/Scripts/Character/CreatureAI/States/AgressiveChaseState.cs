using UnityEngine;

public class AgressiveChaseState : ChaseState
{
    #region serialized fields

    //[Header(nameof(AgressiveChaseState))]
    #endregion

    #region private fields
    #endregion

    public override State SwitchStateInternal()
    {
        if (TimeInState >= 5f)
        {
            return roamState;
            //will flee
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