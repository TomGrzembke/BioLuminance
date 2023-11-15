using UnityEngine;

public class AttackState : State
{
    [Header(nameof(State))]
    #region serialized fields

    public float attackDamage = 0.01f;
    [Space(5)]
    [SerializeField] ChaseState chaseState;
    [SerializeField] RoamState roamState;
    [SerializeField] AttackStanceState attackStanceState;
    [SerializeField] float attackDistance = 2;
    [SerializeField] float maxTimeInState = 1.5f;

    #endregion

    public override State SwitchState()
    {
        if (creatureLogic.DistanceFromTarget > attackDistance)
            return chaseState;
        else if (TimeInState > maxTimeInState)
            return attackStanceState;

        return this;
    }

    protected override void EnterInternal()
    {
        creatureLogic.RefreshAgentVars(creatureLogic.AgentSpeed, creatureLogic.AgentAcceleration, attackDistance);
        creatureLogic.TargetHealthScript.AddHealth(-attackDamage);
    }

    protected override void UpdateInternal()
    {
        creatureLogic.SetDistanceFromTarget(Vector3.Distance(creatureLogic.TargetHealthScript.transform.position, creatureLogic.transform.position));

        HandleRotate();
    }

    protected override void FixedUpdateInternal()
    {
    }

    protected override void ExitInternal()
    {
    }

    private void HandleRotate()
    {
        if (creatureLogic.DistanceFromTarget <= creatureLogic.AgentStoppingDistance)
        {
            Vector3 direction = creatureLogic.TargetHealthScript.transform.position - transform.position;
            direction.z = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.up;
            }

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);

            float step = creatureLogic.AgentAcceleration * Time.deltaTime;
            creatureLogic.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        }
    }
}