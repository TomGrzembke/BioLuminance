using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingrayStinger : MonoBehaviour
{
    #region serialized fields

    [Header("Stinger Info")]

    [SerializeField] Transform stingerGFX;
    [SerializeField] Transform stingerTarget;
    [SerializeField] FlipSpriteOnAngle stingFlipper;
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] ApplyStatusEffects applyStatusEffects;
    [SerializeField] float stingerRadius;

    [Header("Time Info")][SerializeField] float timeToAttack;
    [SerializeField] float cooldownForNextAttack;
    [SerializeField] float attackWindupTime = .7f;
    [SerializeField] float percentAlphaWindupAttackLock = 0.4f;

    [Header("Layer Info")]
    [SerializeField] LayerMask creatureLayer;
    [SerializeField] Creatures creatureType;
    [SerializeField] Creatures targetLayer;

    List<StatusManager> statusTargets = new();
    List<Collider2D> colliderssafe;

    #endregion

    #region private fields

    Coroutine attackCoroutine;
    Coroutine cooldownCoroutine;
    [SerializeField] float rotationMinus;
    #endregion

    void Update()
    {
        HandleDetection();
    }

    void HandleDetection()
    {
        statusTargets.Clear();

        colliderssafe = new(Physics2D.OverlapCircleAll(stingerGFX.position, stingerRadius, creatureLayer));

        GetStatusInColliders(colliderssafe);

        if (statusTargets.Count > 0)
            AttackTarget(statusTargets[Random.Range(0, statusTargets.Count)]);
    }

    void GetStatusInColliders(List<Collider2D> colliders)
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            if (!colliders[i].TryGetComponent(out StatusManager statusTarget))
                statusTarget = colliders[i].GetComponentInParent<StatusManager>();

            if (!statusTarget.TargetLayer.HasFlag(creatureType))
                continue;

            if (!statusTargets.Contains(statusTarget))
                statusTargets.Add(statusTarget);
        }
    }

    public void AttackTarget(StatusManager statusTarget)
    {
        if (cooldownCoroutine == null && attackCoroutine == null)
            attackCoroutine = StartCoroutine(Attack(statusTarget));
    }

    public IEnumerator Attack(StatusManager statusTarget)
    {
        float attackTime = 0;
        float currentAttackWindupTime = 0;
        Transform attackTrans = statusTarget.GrabManager.GetClosestGrabTrans(transform.position);

        Vector3 currentTargetPos = attackTrans.position;
        while (currentAttackWindupTime < attackWindupTime)
        {
            StingerRotationFrame(currentAttackWindupTime, attackTrans.position);

            currentAttackWindupTime += Time.deltaTime;

            if (attackWindupTime * percentAlphaWindupAttackLock < currentAttackWindupTime)
                currentTargetPos = attackTrans.position;
            yield return null;
        }
        Vector3 currentStingPos = stingerTarget.position;

        while (attackTime < timeToAttack)
        {
            StingerRotationFrame(attackTime, attackTrans.position);
            attackTime += Time.deltaTime;
            stingerTarget.position = Vector3.Lerp(currentStingPos, currentTargetPos, animationCurve.Evaluate(attackTime / timeToAttack));
            yield return null;
        }

        cooldownCoroutine = StartCoroutine(Cooldown());
        attackCoroutine = null;
    }

    void StingerRotationFrame(float currentTime, Vector3 attackPos)
    {

        Vector3 vectorToTarget = attackPos - stingerGFX.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - (stingFlipper.Flipped ? rotationMinus : 0);
        Quaternion newQuaternion = Quaternion.AngleAxis(angle, Vector3.forward);

        stingerGFX.rotation = Quaternion.Slerp(stingerGFX.rotation, newQuaternion, currentTime / attackWindupTime);
    }

    public IEnumerator Cooldown()
    {
        float resetTime = 0;
        Vector3 currentPos = stingerTarget.localPosition;
        Quaternion currentRot = stingerGFX.localRotation;

        while (resetTime < cooldownForNextAttack)
        {
            resetTime += Time.deltaTime;
            float progress = resetTime / cooldownForNextAttack;
            stingerTarget.localPosition = Vector3.Lerp(currentPos, Vector2.zero, progress);
            stingerGFX.localRotation = Quaternion.Slerp(currentRot, Quaternion.identity, progress);
            yield return null;
        }

        yield return new WaitForSeconds(cooldownForNextAttack - resetTime);

        cooldownCoroutine = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(stingerGFX.position, stingerRadius);
        Gizmos.DrawLine(stingerGFX.position, stingerTarget.transform.position);
    }
}