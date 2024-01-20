using System.Linq;
using UnityEngine;

public class StunState : State
{
    #region serialized fields
    [SerializeField] ParticleSystem[] stunSystems;
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
        if(stunSystems.Length > 0)
            for (int i = 0; i < stunSystems.Length; i++)
            {
                stunSystems[i].Play();
            }
    }

    protected override void ExitInternal()
    {
        if (stunSystems.Length > 0)
            for (int i = 0; i < stunSystems.Length; i++)
            {
                stunSystems[i].Stop();
            }
    }

    protected override void FixedUpdateInternal()
    {

    }

    protected override void UpdateInternal()
    {

    }
}