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
    
    public override State SwitchState(StateManager enemyManager, AnimationManager enemyAnimationManager)
    {
        //enemyManager.HandleDetection();

        #region Handle switch state

        //if (enemyManager.currentTarget != null)
        //{
        //    return chaseState;
        //}
        //else
        //{
        //    return this;
        //}
        return this;
        #endregion

        //Look for a potential target
        //Switch to chase state if target is found
        //if not return this state
    }

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