using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestState : State
{
    [SerializeField] string a;
    public override State SwitchStateInternal()
    {
        return this;
    }

    protected override void EnterInternal()
    {
        Debug.Log(a);
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