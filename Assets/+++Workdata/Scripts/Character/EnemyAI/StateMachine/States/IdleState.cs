using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    #region serialized fields

    #endregion

    #region private fields

    #endregion
    public override State Tick(NewEnemyAI enemyAI, NewEnemyManager enemyManager, NewEnemyAnimationManager enemyAnimationManager)
    {
        return this;
    }
}