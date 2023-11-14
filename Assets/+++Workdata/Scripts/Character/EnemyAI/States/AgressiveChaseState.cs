public class AgressiveChaseState : ChaseState
{
    public override State SwitchState()
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
            creatureLogic.currentTarget = null;
            creatureLogic.SetCanSeePlayer(false);
            return roamState;
        }

        return this;
    }

    protected override void EnterInternal()
    {
        creatureLogic.RefreshAgentVars(11, 30);
    }

}