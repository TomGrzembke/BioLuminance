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

    protected CreatureLogic creatureLogic;

    #endregion

    void Awake() => creatureLogic = GetComponentInParent<CreatureLogic>();

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
            creatureLogic.currentTarget = null;
            creatureLogic.SetCanSeePlayer(false);
            return roamState;
        }

        return this;
    }

    protected override void EnterInternal()
    {
        creatureLogic.RefreshAgentVars(creatureLogic.EnemySpeed, chaseSpeed, attackRange);
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
         creatureLogic.RefreshAgentVars(creatureLogic.EnemySpeed, 5, creatureLogic.EnemyStoppingDistance);
    }

    private void HandleMovement()
    {
        creatureLogic.SetDistanceFromTarget(Vector3.Distance(creatureLogic.currentTarget.transform.position, creatureLogic.transform.position));

        if (creatureLogic.DistanceFromTarget > creatureLogic.EnemyStoppingDistance)
        {
            creatureLogic.agent.isStopped = false;
        }
        //This is called when the target is close
        else if (creatureLogic.DistanceFromTarget <= creatureLogic.EnemyStoppingDistance)
        {
            creatureLogic.agent.isStopped = true;
            print("Attack");
        }
    }
    private void HandleRotate()
    {
        //Rotate manually
        if (creatureLogic.DistanceFromTarget <= creatureLogic.EnemyStoppingDistance)
        {
            Vector3 direction = creatureLogic.currentTarget.transform.position - transform.position;
            direction.z = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.up;
            }

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);

            float step = creatureLogic.EnemyAcceleration * Time.deltaTime;
            creatureLogic.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        }

        //Rotate with Navmesh
        else
        {
            Vector3 velocity = creatureLogic.agent.velocity;
            velocity.z = 0;
            velocity.Normalize();

            creatureLogic.agent.SetDestination(creatureLogic.currentTarget.transform.position);

            if (velocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, velocity);

                float step = creatureLogic.EnemyAcceleration * Time.deltaTime;
                creatureLogic.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
            }
        }
    }
}