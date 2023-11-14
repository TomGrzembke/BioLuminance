using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoamState : State
{
    [Header(nameof(State))]
    #region serialized fields

    [Space(5)]
    public Vector2 roamPosition;
    public Vector3 startingPosition;
    [SerializeField] float minRoamRange = 5f;
    [SerializeField] float maxRoamRange = 5f;
    [Space(5)]
    [SerializeField] ChaseState chaseState;

    #endregion

    #region private fields

    private CreatureLogic creatureLogic;
    
    #endregion

    private void Awake() => creatureLogic = GetComponentInParent<CreatureLogic>();

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
        return this;
    }

    protected override void EnterInternal()
    {
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
        creatureLogic.enemyStoppingDistance = 0f;
        
        float reachedPositionDistance = 1f;
        
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
        
            float step = creatureLogic.enemyAcceleration * Time.deltaTime;
            creatureLogic.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        }
    }

    public void HandleDetection()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, creatureLogic.detectionRadius, creatureLogic.detectionLayer);
        
        for (int i = 0; i < colliders.Length; i++)
        {
            CreatureLogic creatureLogicc = colliders[i].transform.GetComponent<CreatureLogic>();
        
            if (creatureLogicc != null)
            {
                //It looks for a target on a certain layer, and if that target has the characterStats script, it's added to it's target list
        
                Vector2 targetDirection = (creatureLogicc.transform.position - transform.position).normalized;
        
                if (Vector2.Angle(transform.up, targetDirection) < creatureLogic.angle / 2)
                {
                    if (!Physics2D.Raycast(transform.position, targetDirection, creatureLogic.distanceFromTarget, creatureLogic.obstacleLayer))
                    {
                        creatureLogic.canSeePlayer = true;
                        creatureLogic.currentTarget = creatureLogicc;
                    }
                    else
                    {
                        creatureLogic.canSeePlayer = false;
                        creatureLogic.currentTarget = null;
                    }
                }
                else if (creatureLogic.canSeePlayer)
                    creatureLogic.canSeePlayer = false;
            }
        }
    }
}