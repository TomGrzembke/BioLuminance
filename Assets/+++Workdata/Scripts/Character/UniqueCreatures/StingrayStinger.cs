using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.Serialization;

public class StingrayStinger : MonoBehaviour
{
    [Header("Stinger Info")] public GameObject snapStingerTo;
    public GameObject stinger;
    public GameObject stingerGFX;
    public float stingerRadius;
    [Space(5)] 
    public float stingerDamage;
    public float placeholderHealth;

    [Header("Time Info")] 
    public float timeToAttack;
    public float cooldownForNextAttack;

    [Header("Layer Info")] public LayerMask creatureLayer;
    [SerializeField] Creatures creatureType;
    [SerializeField] Creatures targetLayer;

    [SerializeField] List<StatusManager> statusTargets;
    List<Collider2D> colliders;

    #region serialized fields

    [MinMaxRange(0, 360)] [SerializeField] RangedFloat flipRange = new(0, 180);
    float angle;

    #endregion

    #region private fields

    private Vector2 stingerOriginalPosition;
    
    bool flipped;
    bool inFlipRange;
    bool isCooldown;
    [HideInInspector]public bool isAttacking;
    
    float attackTime;

    #endregion

    private void Start()
    {
        stingerOriginalPosition = stinger.transform.localPosition;
        attackTime = timeToAttack;
    }

    private void Update()
    {
        HandleDetection();
        AttachStinger();
        FlipStinger();
        AttackTarget();
        
        if (statusTargets.Count == 0)
        {
            stinger.transform.localPosition = stingerOriginalPosition;
            stingerGFX.transform.localPosition = Vector3.zero;
            attackTime = timeToAttack;
        }
    }

    public void AttachStinger()
    {
        gameObject.transform.position = snapStingerTo.transform.position;
        gameObject.transform.rotation = snapStingerTo.transform.rotation;
    }

    public void FlipStinger()
    {
        if (isAttacking)
            return;
        
        angle = snapStingerTo.transform.rotation.eulerAngles.z;
        inFlipRange = angle > flipRange.Min && angle < flipRange.Max;

        if (flipped != inFlipRange)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -localScale.x;
            transform.localScale = localScale;

            flipped = inFlipRange;
        }
    }

    public void HandleDetection()
    {
        statusTargets.Clear();

        colliders = new List<Collider2D>(Physics2D.OverlapCircleAll(stinger.transform.position, stingerRadius,
            creatureLayer));

        for (int i = 0; i < colliders.Count; i++)
        {
            if (!colliders[i].TryGetComponent(out StatusManager statusTarget))
                statusTarget = colliders[i].GetComponentInParent<StatusManager>(); // TODO MIT TOMMY BESPRECHEN, GETCOMPONENT PROBLEM

            if (!statusTarget.TargetLayer.HasFlag(creatureType))
                continue;

            if (!statusTargets.Contains(statusTarget))
                statusTargets.Add(statusTarget);
        }
    }

    public void AttackTarget()
    {
        if (isCooldown)
        {
            StartCoroutine(Cooldown());
            return;
        }
        
        if (statusTargets.Count == 0)
            return;

        StartCoroutine(Attack());
    }

    public IEnumerator Attack()
    {
        attackTime -= Time.deltaTime;
        isAttacking = true;
        
        if (attackTime <= 0)
        {
            stingerGFX.transform.position = statusTargets[0].transform.position;
            yield return new WaitForSeconds(0.2f);
            isCooldown = true;
            isAttacking = false;
        }
    }

    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownForNextAttack);
        isCooldown = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(stinger.transform.position, stingerRadius);
        Gizmos.DrawLine(stinger.transform.position, stingerGFX.transform.position);
    }
}