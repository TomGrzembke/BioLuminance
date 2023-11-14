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

    #region private fields

    CreatureLogic creatureLogic;

    #endregion

    void Awake() => creatureLogic = GetComponentInParent<CreatureLogic>();

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
        creatureLogic.RefreshAgentVars(creatureLogic.EnemySpeed, creatureLogic.EnemyAcceleration, attackDistance);
    }

    protected override void UpdateInternal()
    {
        creatureLogic.SetDistanceFromTarget(Vector3.Distance(creatureLogic.currentTarget.transform.position, creatureLogic.transform.position));

        creatureLogic.currentTarget.GetComponent<Health>().AddHealth(-attackDamage);

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
    }
}