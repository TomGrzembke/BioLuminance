using UnityEngine;

public class ChaseState : State
{
    [Header(nameof(State))]
    #region serialized fields

    [SerializeField] protected float chaseRange;
    [SerializeField] protected float attackRange = 2;
    [SerializeField] protected float chaseSpeed = 2;

    [SerializeField] protected AttackState attackState;
    [SerializeField] protected RoamState roamState;
    [SerializeField] AgressiveChaseState agressiveChaseState;
    #endregion

    #region private fields
    #endregion

    public override State SwitchState()
    {
        if (TimeInState >= 5f)
        {
            return agressiveChaseState;
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
        creatureLogic.RefreshAgentVars(creatureLogic.AgentSpeed, chaseSpeed, attackRange);
    }

    protected override void UpdateInternal()
    {
        HandleMovement();
        HandleRotate();
    }

    protected override void FixedUpdateInternal()
    {
    }

    protected override void ExitInternal()
    {
        creatureLogic.RefreshAgentVars(creatureLogic.AgentSpeed, 5, creatureLogic.AgentStoppingDistance);
    }

    private void HandleMovement()
    {
        creatureLogic.SetDistanceFromTarget(Vector3.Distance(creatureLogic.TargetHealthScript.transform.position, creatureLogic.transform.position));

        if (creatureLogic.DistanceFromTarget > creatureLogic.AgentStoppingDistance)
        {
            creatureLogic.agent.isStopped = false;
        }
        //This is called when the target is close
        else if (creatureLogic.DistanceFromTarget <= creatureLogic.AgentStoppingDistance)
        {
            creatureLogic.agent.isStopped = true;
            print("Attack");
        }
    }
    private void HandleRotate()
    {
        //Rotate manually
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

        //Rotate with Navmesh
        else
        {
            Vector3 velocity = creatureLogic.agent.velocity;
            velocity.z = 0;
            velocity.Normalize();

            creatureLogic.agent.SetDestination(creatureLogic.TargetHealthScript.transform.position);

            if (velocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, velocity);

                float step = creatureLogic.AgentAcceleration * Time.deltaTime;
                creatureLogic.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
            }
        }
    }
}