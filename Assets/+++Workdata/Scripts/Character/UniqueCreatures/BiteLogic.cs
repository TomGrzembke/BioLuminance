using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteLogic : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Collider2D attackHitbox;
    [SerializeField] ContactFilter2D contactFilter;
    [SerializeField] StatusManager ownStatusManager;
    [SerializeField] ApplyStatusEffects applyStatusEffects;
    [SerializeField] StatusEffects statusEffects;
    [SerializeField] float timeTillSpeedRemoval = 2;
    [SerializeField] Animator anim;
    [SerializeField] AnimationClip animClip;
    #endregion

    #region private fields
    List<Collider2D> colliders = new();
    #endregion

    void Awake()
    {
        anim = GetComponent<Animator>();
        contactFilter.layerMask = LayerMask.GetMask("Creature");
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = true;
    }

    void Update()
    {
        HandleDetection();
    }

    void HandleDetection()
    {
        if (Physics2D.OverlapCollider(attackHitbox, contactFilter, colliders) > 0)
            for (int i = 0; i < colliders.Count; i++)
            {
                if (!colliders[i].TryGetComponent(out LimbSubject _limbTarget)) continue;

                if (_limbTarget == ownStatusManager) continue;

                applyStatusEffects.ApplyEffects(statusEffects, _limbTarget, ownStatusManager);

                if (statusEffects.speedModifier)
                    StartCoroutine(RemoveSpeedModifierCor(_limbTarget, statusEffects.speedModifier));
            }
    }

    IEnumerator RemoveSpeedModifierCor(LimbSubject _limbTarget, SpeedModifier _speedModifier)
    {
        yield return new WaitForSeconds(timeTillSpeedRemoval);
        applyStatusEffects.RemoveSpeedModifier(_limbTarget.OwnStatusManager, _speedModifier);
    }

    public void SetOwnStatusManager(StatusManager statusManager)
    {
        ownStatusManager = statusManager;
        applyStatusEffects = statusManager.ApplyStatusEffects;
    }

    public void Bite()
    {
        if(!anim)
            anim = GetComponent<Animator>();
        anim.Play(animClip.name, 0, 0f);
    }
}