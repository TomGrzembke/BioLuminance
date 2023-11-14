using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgressiveChaseState : State
{
    [Header(nameof(State))]
    #region serialized fields

    [SerializeField] float chaseRange;

    [SerializeField] AttackState attackState;
    [SerializeField] RoamState roamState;

    #endregion

    #region private fields

    CreatureLogic creatureLogic;
    
    #endregion

    private void Awake() => creatureLogic = GetComponentInParent<CreatureLogic>();

    public override State SwitchState()
    { 
        #region Handle switch state
        if (TimeInState >= 5f)
        {
            return roamState;
        }
        if (creatureLogic.distanceFromTarget <= creatureLogic.enemyStoppingDistance)
        {
            return attackState;
        }
        else if(creatureLogic.distanceFromTarget >= chaseRange)
        {
            //creatureLogic.currentTarget = null;
            creatureLogic.canSeePlayer = false;
            return roamState;
        }
        else
        {
            return this;
        }
        #endregion
    }

    protected override void EnterInternal()
    {
        creatureLogic.enemySpeed = 11f;
        creatureLogic.enemyAcceleration = 30f;
    }

    protected override void UpdateInternal()
    {
        HandleMovement(); 
        HandleRotate();
    }

    protected override void FixedUpdateInternal()
    {
    }

    protected override void ExitInternal()
    {
        creatureLogic.enemySpeed = 3.5f;
        creatureLogic.enemyAcceleration = 5f;
    }
    
    private void HandleMovement()
    {
        Vector3 targetDirection = creatureLogic.currentTarget.transform.position - transform.position;
        creatureLogic.distanceFromTarget = Vector3.Distance(creatureLogic.currentTarget.transform.position, creatureLogic.transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.position);
        creatureLogic.enemyStoppingDistance = 1.5f;
        creatureLogic.enemySpeed = 10f;
        
        if (creatureLogic.distanceFromTarget > creatureLogic.enemyStoppingDistance)
        {
            creatureLogic.agent.isStopped = false;
        }
        //This is called when the target is close
        else if (creatureLogic.distanceFromTarget <= creatureLogic.enemyStoppingDistance)
        {
            creatureLogic.agent.isStopped = true;
            print("Attack");
        }
    }
    private void HandleRotate()
    {
        //Rotate manually
        if (creatureLogic.distanceFromTarget <= creatureLogic.enemyStoppingDistance)
        {
            Vector3 direction = creatureLogic.currentTarget.transform.position - transform.position;
            direction.z = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.up;
            }

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);

            float step = creatureLogic.enemyAcceleration * Time.deltaTime;
            creatureLogic.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        }

        //Rotate with Navmesh
        else
        {
            Vector3 velocity = creatureLogic.agent.velocity;
            velocity.z = 0;
            velocity.Normalize();

            creatureLogic.agent.SetDestination(creatureLogic.currentTarget.transform.position);

            if (velocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, velocity);

                float step = creatureLogic.enemyAcceleration * Time.deltaTime;
                creatureLogic.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
            }
        }
    }
}