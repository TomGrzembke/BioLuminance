using UnityEngine;

public class AttackState : State
{
    #region serialized fields
    [Header(nameof(AttackState))]

    [SerializeField] float attackDamage = 0.01f;
    [Space(5)]
    [SerializeField] ChaseState chaseState;
    [SerializeField] RoamState roamState;
    [SerializeField] AttackStanceState attackStanceState;
    [SerializeField] float maxTimeInState = 1.5f;
    #endregion

    public override State SwitchStateInternal()
    {
        if (creatureLogic.DistanceFromTarget > stateAgentStoppingDistance)
            return chaseState;
        else if (TimeInState > maxTimeInState)
            return attackStanceState;

        return this;
    }

    protected override void EnterInternal()
    {
        creatureLogic.TargetHealthScript.AddHealth(-attackDamage);
    }

    protected override void UpdateInternal()
    {
        creatureLogic.SetDistanceFromTarget(Vector3.Distance(creatureLogic.TargetHealthScript.transform.position, creatureLogic.transform.position));

        creatureLogic.HandleRotate();
    }

    protected override void FixedUpdateInternal()
    {
    }

    protected override void ExitInternal()
    {
    }
}