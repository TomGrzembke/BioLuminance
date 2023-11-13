using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    #region serialized fields

    #endregion

    #region private fields

    #endregion
    public override State SwitchState()
    {
        //Select one attack
        //if the selected is not able to be able to be used because of bad angle or distance, select a new attack
        //if the attack is viable, stop our movement and attack our target
        //set out recovery timer to the attacks recovery time
        //return the attack stance state
        
        return this;
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