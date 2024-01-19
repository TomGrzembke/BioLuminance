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
    #endregion

    #region private fields
    List<Collider2D> colliders = new();
    #endregion

    void Awake()
    {
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
            }
    }
}