using UnityEngine;

public class AgressiveChaseState : ChaseState
{
    #region serialized fields

    [Header(nameof(AgressiveChaseState))]
    [SerializeField] float agentSpeed = 10;
    [SerializeField] float agentAcceleration = 30;
    #endregion

    #region private fields
    float oldAgentSpeed;
    float oldAgentAcceleration;
    #endregion

    public override State SwitchStateInternal()
    {
        if (TimeInState >= 5f)
        {
            return roamState;
            //will flee
        }
        if (creatureLogic.DistanceFromTarget <= attackRange)
        {
            return attackState;
        }
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
        oldAgentSpeed = creatureLogic.AgentSpeed;
        oldAgentAcceleration = creatureLogic.AgentAcceleration;
        creatureLogic.RefreshAgentVars(agentSpeed, agentAcceleration);
    }

    protected override void ExitInternal()
    {
        creatureLogic.RefreshAgentVars(oldAgentSpeed, oldAgentAcceleration);
    }

}