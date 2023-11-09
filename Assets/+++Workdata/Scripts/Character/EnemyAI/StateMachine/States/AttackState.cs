using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    #region serialized fields

    #endregion

    #region private fields

    #endregion
    public override State Tick(NewEnemyManager enemyManager, NewEnemyAnimationManager enemyAnimationManager, EnemyStats enemyStats)
    {
        //Select one attack
        //if the selected is not able to be able to be used because of bad angle or distance, select a new attack
        //if the attack is viable, stop our movement and attack our target
        //set out recovery timer to the attacks recovery time
        //return the attack stance state

        return this;
    }
}