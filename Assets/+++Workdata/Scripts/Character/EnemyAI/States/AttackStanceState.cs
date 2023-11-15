using NavMeshPlus.Extensions;
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
        if (creatureLogic.agent.hasPath) return;

        int pathHorizontal = Random.Range(-1, 2);
        int pathVertical = Random.Range(-1, 2);
        Vector3 pathAddVec3 = new(pathVertical, pathHorizontal);

        creatureLogic.agent.SetDestination(creatureLogic.TargetHealthScript.transform.position + pathAddVec3 * 10);
    }

    protected override void FixedUpdateInternal()
    {
        creatureLogic.HandleRotate();
    }

    protected override void ExitInternal()
    {
    }

}