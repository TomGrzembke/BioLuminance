using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    #region serialized fields

    #endregion

    #region private fields

    #endregion

    public abstract State Tick(NewEnemyAI enemyAI, NewEnemyManager enemyManager, NewEnemyAnimationManager enemyAnimationManager);

}