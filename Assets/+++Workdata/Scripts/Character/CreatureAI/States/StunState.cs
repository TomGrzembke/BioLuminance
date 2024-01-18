public class StunState : State
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
        creatureLogic.agent.velocity = new(0, 0, 0);
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