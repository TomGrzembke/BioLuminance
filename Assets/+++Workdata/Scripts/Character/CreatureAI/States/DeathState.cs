using UnityEngine;

public class DeathState : State
{
    #region serialized fields
    [SerializeField] StateManager stateManager;
    #endregion

    #region private fields

    #endregion

    public override State SwitchStateInternal()
    {
        return this;
    }

    protected override void EnterInternal()
    {
        stateManager.enabled = false;
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