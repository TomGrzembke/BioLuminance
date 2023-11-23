using UnityEngine;

public class DeathState : State
{
    #region serialized fields

    #endregion

    #region private fields

    #endregion

    public override State SwitchStateInternal()
    {
        return this;
    }

    protected override void EnterInternal()
    {
    }

    protected override void ExitInternal()
    {
    }

    protected override void FixedUpdateInternal()
    {
    }

    protected override void UpdateInternal()
    {
    }
}