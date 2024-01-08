using System.Collections.Generic;
using UnityEngine;
using static TentacleDetection;

public class AttackState : State
{
    #region serialized fields
    [Header(nameof(AttackState))]

    [SerializeField] float attackDamage = 0.01f;
    [Space(5)]
    [SerializeField] ChaseState chaseState;
    [SerializeField] RoamState roamState;
    [SerializeField] AttackStanceState attackStanceState;
    [SerializeField] Collider2D attackHitbox;
    [SerializeField] ContactFilter2D contactFilter;
    [SerializeField] StatusManager ownStatusManager;
    [SerializeField] float maxTimeInState = 1.5f;
    #endregion

    #region private fields
    List<Collider2D> colliders = new();
    #endregion
    public override State SwitchStateInternal()
    {
        if (creatureLogic.DistanceFromTarget > stateAgentStoppingDistance)
            return chaseState;
        else if (TimeInState > maxTimeInState)
            return attackStanceState;

        return this;
    }

    protected override void EnterInternal()
    {    
    }

    protected override void UpdateInternal()
    {
        HandleDetection();

        creatureLogic.SetDistanceFromTarget(Vector3.Distance(creatureLogic.TargetStatusManager.transform.position, creatureLogic.transform.position));

        creatureLogic.HandleRotate();
    }

    protected override void FixedUpdateInternal()
    {
    }

    protected override void ExitInternal()
    {
    }

    void HandleDetection()
    {
        if (Physics2D.OverlapCollider(attackHitbox, contactFilter, colliders) < 0) return;

        for (int i = 0; i < colliders.Count; i++)
        {
            if (!colliders[i].TryGetComponent(out LimbSubject _limbTarget)) continue;

            if (_limbTarget == ownStatusManager) continue;

            _limbTarget.AddDamage(attackDamage);
        }
    }
}