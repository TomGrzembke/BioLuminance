using UnityEngine;

public class AttackStanceState : State
{
    #region serialized fields

    [SerializeField] State chaseState;
    [SerializeField] State roamState;
    [SerializeField] float stanceTime = 5;
    [SerializeField] float acceleration = 7;

    #endregion

    #region private fields

    #endregion
    public override State SwitchState()
    {
        if (TimeInState > stanceTime)
            return chaseState;

        return this;
    }

    protected override void EnterInternal()
    {
        creatureLogic.RefreshAgentVars(creatureLogic.AgentSpeed, acceleration, 0);
    }

    protected override void UpdateInternal()
    {
        creatureLogic.agent.SetDestination(transform.position * Random.Range(-1,1));
    }

    protected override void FixedUpdateInternal()
    {
    }

    protected override void ExitInternal()
    {
    }

}