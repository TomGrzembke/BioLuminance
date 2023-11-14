using UnityEngine;
using Random = UnityEngine.Random;

public class RoamState : State
{
    [Header(nameof(State))]
    #region serialized fields

    [SerializeField] float minRoamRange = 5f;
    [SerializeField] float maxRoamRange = 5f;
    [SerializeField] ChaseState chaseState;
    [SerializeField]  float reachedPositionDistance = 1;

    #endregion

    #region private fields
    Vector2 roamPosition;
    Vector3 startingPosition;
    CreatureLogic creatureLogic;
    float oldStoppingDistance;
    #endregion

    void Awake() => creatureLogic = GetComponentInParent<CreatureLogic>();

    public override State SwitchState()
    {
        if (creatureLogic.currentTarget != null)
        {
            return chaseState;
        }
        else
        {
            return this;
        }
    }

    protected override void EnterInternal()
    {
        oldStoppingDistance = creatureLogic.EnemyStoppingDistance;
        creatureLogic.RefreshAgentVars(creatureLogic.EnemySpeed, creatureLogic.EnemyAcceleration, 0);
    }

    protected override void UpdateInternal()
    {
        HandleDetection();
        HandleRoaming();
        HandleRotate();
    }

    protected override void FixedUpdateInternal()
    {
    }

    protected override void ExitInternal()
    {
        creatureLogic.RefreshAgentVars(creatureLogic.EnemySpeed, creatureLogic.EnemyAcceleration, oldStoppingDistance);
    }

    void Start()
    {
        startingPosition = transform.position;
        roamPosition = startingPosition;
    }

    Vector3 GetRandomRoamingPosition()
    {
        return startingPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(minRoamRange, maxRoamRange);
    }

    private void HandleRoaming()
    {
        creatureLogic.agent.SetDestination(roamPosition);

        if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
        {
            roamPosition = GetRandomRoamingPosition();
        }
    }
    private void HandleRotate()
    {
        Vector3 velocity = creatureLogic.agent.velocity;
        velocity.z = 0;
        velocity.Normalize();

        if (velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, velocity);

            float step = creatureLogic.EnemyAcceleration * Time.deltaTime;
            creatureLogic.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        }
    }

    public void HandleDetection()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, creatureLogic.DetectionRadius, creatureLogic.DetectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CreatureLogic _creatureLogic = colliders[i].transform.GetComponent<CreatureLogic>();

            if (_creatureLogic != null)
            {
                //It looks for a target on a certain layer, and if that target has the characterStats script, it's added to it's target list

                Vector2 targetDirection = (_creatureLogic.transform.position - transform.position).normalized;

                if (Vector2.Angle(transform.up, targetDirection) < creatureLogic.Angle / 2)
                {
                    if (!Physics2D.Raycast(transform.position, targetDirection, creatureLogic.DistanceFromTarget, this.creatureLogic.ObstacleLayer))
                    {
                        creatureLogic.SetCanSeePlayer(true);
                        creatureLogic.currentTarget = _creatureLogic;
                    }
                    else
                    {
                        creatureLogic.SetCanSeePlayer(false);
                        creatureLogic.currentTarget = null;
                    }
                }
                else if (creatureLogic.CanSeePlayer)
                    creatureLogic.SetCanSeePlayer(false);
            }
        }
    }
}