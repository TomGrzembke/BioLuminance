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
    
    public override State Tick(NewEnemyManager enemyManager, NewEnemyAnimationManager enemyAnimationManager, EnemyStats enemyStats)
    {
        enemyManager.HandleDetection();

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