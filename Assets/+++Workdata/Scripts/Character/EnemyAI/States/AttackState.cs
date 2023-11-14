using UnityEngine;

public class AttackState : State
{
    [Header(nameof(State))]
    #region serialized fields

    public float attackDamage = 0.01f;
    [Space(5)]
    public ChaseState chaseState;
    public RoamState roamState;
    [SerializeField] float attackDistance = 2;

    #endregion

    public override State SwitchState()
    {
        if (creatureLogic.DistanceFromTarget > attackDistance)
        {
            return chaseState;
        }
        return this;
    }

    protected override void EnterInternal()
    {
        creatureLogic.RefreshAgentVars(creatureLogic.AgentSpeed, creatureLogic.AgentAcceleration, attackDistance);
    }

    protected override void UpdateInternal()
    {
        creatureLogic.SetDistanceFromTarget(Vector3.Distance(creatureLogic.TargetHealthScript.transform.position, creatureLogic.transform.position));

        creatureLogic.TargetHealthScript.GetComponent<Health>().AddHealth(-attackDamage);

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