using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    #region serialized fields

    #endregion

    #region private fields

    #endregion

    public abstract State Tick(NewEnemyManager enemyManager, NewEnemyAI enemyAI, NewEnemyAnimationManager enemyAnimationManager, EnemyStats enemyStats);

}