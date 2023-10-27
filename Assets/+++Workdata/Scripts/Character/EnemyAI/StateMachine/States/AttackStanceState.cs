using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStanceState : State
{
    #region serialized fields

    public ChaseState chaseState;

    #endregion

    #region private fields

    #endregion
    public override State Tick(NewEnemyManager enemyManager, NewEnemyAI enemyAI, NewEnemyAnimationManager enemyAnimationManager, EnemyStats enemyStats)
    {
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        enemyManager.enemyStoppingDistance = 3f;
        
        HandleRotate(enemyManager);

        #region Handle switch state

        if (enemyManager.distanceFromTarget > enemyManager.enemyStoppingDistance)
        {
            return chaseState;
        }
        else
        {
            return this;
        }

        #endregion
        //Check for attack range
        //potentially circle player or walk around them
        //if in attack range return attack State
        //if we are in a cooldown after attacking, return this state and continue circling player
        //if the player runs out of range return chase state
        return this;
    }

    private void HandleRotate(NewEnemyManager enemyManager)
    {
        //rotate manually
        if (enemyManager.isPerformingAction || enemyManager.distanceFromTarget <= enemyManager.enemyStoppingDistance)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
            direction.z = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.up;
            }

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);

            float step = enemyManager.enemyAcceleration * Time.deltaTime;
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        }
    }
}