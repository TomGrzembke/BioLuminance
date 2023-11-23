using MyBox;
using UnityEngine;

public class FleeState : State
{
    #region serialized fields
    [MinMaxRange(0, 10)]
    [SerializeField] RangedFloat randomMoveLength = new(0, 10);

    #endregion

    #region private fields

    #endregion

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
  
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
        creatureLogic.HandleRotate();
    }

    protected override void UpdateInternal()
    {
        if (creatureLogic.agent.hasPath) return;

        int pathHorizontal = Random.Range(-1, 2);
        int pathVertical = Random.Range(-1, 2);
        Vector3 pathAddVec3 = new(pathVertical, pathHorizontal);

        float randomMultiplier = Random.Range(randomMoveLength.Min, randomMoveLength.Max);
        creatureLogic.agent.SetDestination(transform.position + pathAddVec3 * randomMultiplier);
    }
}