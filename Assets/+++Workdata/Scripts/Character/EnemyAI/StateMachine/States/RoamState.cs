using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamState : State
{
    #region serialized fields

    [SerializeField] LayerMask detectionLayer;
    [Space(5)]
    public Vector2 roamPosition;
    public Vector3 startingPosition;
    [SerializeField] float minRoamRange = 5f;
    [SerializeField] float maxRoamRange = 5f;
    [Space(5)]
    [SerializeField] ChaseState chaseState;


    #endregion

    #region private fields

    #endregion

    public override State Tick(NewEnemyManager enemyManager, NewEnemyAI enemyAI, NewEnemyAnimationManager enemyAnimationManager, EnemyStats enemyStats)
    {
        enemyManager.distanceFromTarget = 0;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                //It looks for a target on a certain layer, and if that target has the characterStats script, it's added to it's target list

                Vector2 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector2.Angle(targetDirection, transform.up);

                if (viewableAngle > enemyManager.minDetectionAngle && viewableAngle < enemyManager.maxDetectionAngle)
                {
                    enemyManager.currentTarget = characterStats;
                }
            }
        }

        HandleRoaming(enemyManager);
        HandleRotateTowardsTarget(enemyManager);

        //Look for a potential target
        //Switch to chase state if target is found

        if (enemyManager.currentTarget != null)
        {
            return chaseState;
        }
        else
        {
            return this;
        }
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

    private void HandleRoaming(NewEnemyManager enemyManager)
    {
        enemyManager.agent.SetDestination(roamPosition);
        enemyManager.enemyStoppingDistance = 0f;

        float reachedPositionDistance = 1f;

        if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
        {
            roamPosition = GetRandomRoamingPosition();
        }
    }

    private void HandleRotateTowardsTarget(NewEnemyManager enemyManager)
    {
        Vector3 velocity = enemyManager.agent.velocity;
        velocity.z = 0;
        velocity.Normalize();

        if (velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, velocity);

            float step = enemyManager.enemyAcceleration * Time.deltaTime;
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        }
    }
}