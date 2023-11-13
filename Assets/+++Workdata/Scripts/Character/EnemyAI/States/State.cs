using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract State SwitchState();
    
    /// <summary>
    /// The time since the state was entered.
    /// </summary>
    protected float TimeInState;
    
    /// <summary>
    /// The fixed time since the state was entered, for any physics tests.
    /// </summary>
    protected float FixedTimeInState;
    
    public void EnterState()
    {
        TimeInState = 0;
        FixedTimeInState = 0;
        EnterInternal();
    }

    
    protected abstract void EnterInternal();

    public void UpdateState()
    {
        TimeInState += Time.deltaTime;
        UpdateInternal();
    }

    protected abstract void UpdateInternal();

    public void FixedUpdateState()
    {
        FixedTimeInState += Time.fixedDeltaTime;
        FixedUpdateInternal();
    }

    protected abstract void FixedUpdateInternal();

    public void ExitState()
    {
        ExitInternal();
    }

    protected abstract void ExitInternal();
}