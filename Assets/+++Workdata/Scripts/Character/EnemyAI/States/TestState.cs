using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestState : State
{
    public override State SwitchState()
    {
        return this;
    }

    protected override void EnterInternal()
    {
        Debug.Log("Enter");
    }

    protected override void UpdateInternal()
    {
        Debug.Log("Update");
    }

    protected override void FixedUpdateInternal()
    {
        Debug.Log("FixedUpdate");
    }

    protected override void ExitInternal()
    {
        Debug.Log("Exit");
    }
}