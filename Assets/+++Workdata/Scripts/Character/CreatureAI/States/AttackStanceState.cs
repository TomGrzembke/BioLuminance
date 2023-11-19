using MyBox;
using UnityEngine;

public class AttackStanceState : State
{
    #region serialized fields
    [Header(nameof(AttackStanceState))]
    [SerializeField] State chaseState;
    [SerializeField] State roamState;
    [SerializeField] float stanceTime = 5;

    [MinMaxRange(0, 10)]
    [SerializeField] RangedFloat randomMoveLength = new(0,10);

    #endregion

    #region private fields

    #endregion

    public override State SwitchStateInternal()
    {
        if (TimeInState > stanceTime)
            return chaseState;

        return this;
    }

    protected override void EnterInternal()
    {
    }

    protected override void UpdateInternal()
    {
        if (creatureLogic.agent.hasPath) return;

        int pathHorizontal = Random.Range(-1, 2);
        int pathVertical = Random.Range(-1, 2);
        Vector3 pathAddVec3 = new(pathVertical, pathHorizontal);

        float randomMultiplier = Random.Range(randomMoveLength.Min, randomMoveLength.Max);
        creatureLogic.agent.SetDestination(creatureLogic.TargetHealthScript.transform.position + pathAddVec3 * randomMultiplier);
    }

    protected override void FixedUpdateInternal()
    {
        creatureLogic.HandleRotate();
    }

    protected override void ExitInternal()
    {
    }

}