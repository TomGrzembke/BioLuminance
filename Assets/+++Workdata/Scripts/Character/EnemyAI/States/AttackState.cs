using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    [Header(nameof(State))]
     #region serialized fields

    public float attackDamage;
    [Space(5)]
    public ChaseState chaseState;
    public RoamState roamState;

    #endregion

    #region private fields

    CreatureLogic creatureLogic;
    
    #endregion

    void Awake() => creatureLogic = GetComponentInParent<CreatureLogic>();
    
    public override State SwitchState()
    {
        #region Handle switch state

        if (creatureLogic.distanceFromTarget > creatureLogic.enemyStoppingDistance)
        {
            return chaseState;
        }
        else
        {
            return this;
        }

        #endregion

        return this;
    }

    protected override void EnterInternal()
    {
    }

    protected override void UpdateInternal()
    {
        creatureLogic.distanceFromTarget = Vector3.Distance(creatureLogic.currentTarget.transform.position, creatureLogic.transform.position);
        creatureLogic.enemyStoppingDistance = 3f;
        
        //creatureLogic.currentTarget.GetComponent<Health>().CurrentHealth -= 5;
        creatureLogic.currentTarget.GetComponent<Health>().AddHealth(-attackDamage);
        
        HandleRotate();
    }

    protected override void FixedUpdateInternal()
    {
    }

    protected override void ExitInternal()
    {
    }

    private void HandleRotate()
    {
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
    }
}