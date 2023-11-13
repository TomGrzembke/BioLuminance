using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    #region serialized fields

    [SerializeField] float chaseRange;

    [SerializeField] AttackStanceState attackStanceState;
    [SerializeField] RoamState roamState;

    #endregion

    #region private fields

    #endregion

    public override State SwitchState(StateManager enemyManager, AnimationManager enemyAnimationManager)
    {
    //    HandleMovement(enemyManager);
    //    HandleRotate(enemyManager);

        #region Handle switch state

        //if (enemyManager.distanceFromTarget <= enemyManager.enemyStoppingDistance)
        //{
        //    return attackStanceState;
        //}
        //else if(enemyManager.distanceFromTarget >= chaseRange)
        //{
        //    enemyManager.currentTarget = null;
        //    enemyManager.canSeePlayer = false;
        //    return roamState;
        //}
        //else
        //{
        //    return this;
        //}
        return this;
        #endregion

        //Chase the target
        //If within attack range, switch to attack stance state
        //if target is out of range, return this state and continue to chase target
    }

    //private void HandleMovement(StateManager enemyManager)
    //{
    //    if (enemyManager.isPerformingAction)
    //        return;

    //    Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
    //    enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
    //    float viewableAngle = Vector3.Angle(targetDirection, transform.position);
    //    enemyManager.enemyStoppingDistance = 1.5f;

    //    if (enemyManager.isPerformingAction)
    //    {
    //        enemyManager.agent.isStopped = true;
    //        print("performs action");
    //    }
    //    else
    //    {
    //        if (enemyManager.distanceFromTarget > enemyManager.enemyStoppingDistance)
    //        {
    //            enemyManager.agent.isStopped = false;
    //        }
    //        //This is called when the target is close
    //        else if (enemyManager.distanceFromTarget <= enemyManager.enemyStoppingDistance)
    //        {
    //            enemyManager.agent.isStopped = true;
    //            print("Attack");
    //        }
    //    }
    //}
    //private void HandleRotate(StateManager enemyManager)
    //{
    //    //Rotate manually
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

    //    //Rotate with Navmesh
    //    else
    //    {
    //        Vector3 velocity = enemyManager.agent.velocity;
    //        velocity.z = 0;
    //        velocity.Normalize();

    //        enemyManager.agent.SetDestination(enemyManager.currentTarget.transform.position);

    //        if (velocity != Vector3.zero)
    //        {
    //            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, velocity);

    //            float step = enemyManager.enemyAcceleration * Time.deltaTime;
    //            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
    //        }
    //    }
    //}

    protected override void EnterInternal()
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdateInternal()
    {
        throw new System.NotImplementedException();
    }

    protected override void FixedUpdateInternal()
    {
        throw new System.NotImplementedException();
    }

    protected override void ExitInternal()
    {
        throw new System.NotImplementedException();
    }
}