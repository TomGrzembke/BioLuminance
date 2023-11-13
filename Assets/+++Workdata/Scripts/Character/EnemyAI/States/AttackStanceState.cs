using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStanceState : State
{
    #region serialized fields

    public float attackDamage;
    [Space(5)]
    public ChaseState chaseState;
    public RoamState roamState;

    #endregion

    #region private fields

    #endregion
    public override State Tick(StateManager enemyManager, AnimationManager enemyAnimationManager)
    {
        //enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        //enemyManager.enemyStoppingDistance = 3f;
        
        ////enemyManager.currentTarget.GetComponent<Health>().CurrentHealth -= 5;
        //enemyManager.currentTarget.GetHealth().AddHealth(-attackDamage);
        
        //HandleRotate(enemyManager);

        //#region Handle switch state

        //if (enemyManager.distanceFromTarget > enemyManager.enemyStoppingDistance)
        //{
        //    return chaseState;
        //}
        //else
        //{
        //    return this;
        //}

        //#endregion
        ////Check for attack range
        ////potentially circle player or walk around them
        ////if in attack range return attack State
        ////if we are in a cooldown after attacking, return this state and continue circling player
        ////if the player runs out of range return chase state
        return this;
    }

    //private void HandleRotate(StateManager enemyManager)
    //{
    //    //rotate manually
    //    if (enemyManager.isPerformingAction || enemyManager.distanceFromTarget <= enemyManager.enemyStoppingDistance)
    //    {
    //        Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
    //        direction.z = 0;
    //        direction.Normalize();

    //        if (direction == Vector3.zero)
    //        {
    //            direction = transform.up;
    //        }

    //        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);

    //        float step = enemyManager.enemyAcceleration * Time.deltaTime;
    //        enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
    //    }
    //}
}