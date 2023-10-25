using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    #region serialized fields

    public ChaseState chaseState;
    [SerializeField] LayerMask detectionLayer;

    #endregion

    #region private fields

    #endregion
    public override State Tick(NewEnemyManager enemyManager, NewEnemyAI enemyAI, NewEnemyAnimationManager enemyAnimationManager, EnemyStats enemyStats)
    {
        #region Handle enemy detection

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

        #endregion

        #region Handle switch state

        if (enemyManager.currentTarget != null)
        {
            return chaseState;
        }
        else
        {
            return this;
        }

        #endregion

        //Look for a potential target
        //Switch to chase state if target is found
        //if not return this state
    }
}