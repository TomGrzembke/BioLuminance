using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunfishAttackState : State
{
    #region serialized fields
    [Header(nameof(AttackState))]

    [Space(5)]
    [SerializeField] ChaseState chaseState;
    [SerializeField] RoamState roamState;
    [SerializeField] Collider2D reachHitbox;
    [SerializeField] ContactFilter2D contactFilter;
    [SerializeField] StatusManager ownStatusManager;
    [SerializeField] Collider2D attackHitbox;
    [SerializeField] AnimationClip animClip;
    [SerializeField] float cooldown = 3;
    [SerializeField] float chaseDistance = 40;
    [SerializeField] Animator animator;
    #endregion

    #region private fields
    List<Collider2D> colliders = new();
    Coroutine cooldownCor;
    #endregion

    void Awake()
    {
        contactFilter.layerMask = LayerMask.GetMask("Creature");
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = true;
    }

    public override State SwitchStateInternal()
    {
        if (creatureLogic.DistanceFromTarget > chaseDistance)
            return chaseState;

        return this;
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

    protected override void EnterInternal()
    {

    }
    void HandleDetection()
    {
        if (Physics2D.OverlapCollider(reachHitbox, contactFilter, colliders) < 0) return;

        if (cooldownCor != null) return;

        cooldownCor = StartCoroutine(Cooldown());

        bool bite = false;

        for (int i = 0; i < colliders.Count; i++)
        {
            if (!colliders[i].TryGetComponent(out LimbSubject _limbTarget)) continue;

            if (_limbTarget == ownStatusManager) continue;

            if (!bite)
                bite = true;
        }

        if (bite)
            animator.Play(animClip.name, 0, 0f);
    }

    IEnumerator Cooldown()
    {
        float wentByTime = 0;
        while (wentByTime < cooldown)
        {
            wentByTime += Time.deltaTime;
            yield return null;
        }

        cooldownCor = null;
    }

}