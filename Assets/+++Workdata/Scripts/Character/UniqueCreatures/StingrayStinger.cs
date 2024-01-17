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
    [SerializeField] StatusManager ownStatusManager;
    [SerializeField] StatusEffects statusEffects;
    [SerializeField] ContactFilter2D contactFilter;
    [SerializeField] float stingerRadius;

    [Header("Time Info")][SerializeField] float timeToAttack = 1;
    [SerializeField] float cooldownForNextAttack = 2;
    [SerializeField] float attackWindupTime = .7f;
    [SerializeField] float percentAlphaWindupAttackLock = 0.4f;

    [Header("Layer Info")]
    [SerializeField] LayerMask creatureLayer;

    ApplyStatusEffects applyStatusEffects;
    List<LimbSubject> limbTargets = new();
    List<Collider2D> colliderTargets;
    List<Collider2D> colliderAttack = new();

    #endregion

    #region private fields

    Collider2D stingCollider;
    Coroutine attackCoroutine;
    Coroutine cooldownCoroutine;
    Coroutine hitDetectCoroutine;
    [SerializeField] float rotationMinus;
    #endregion

    void Awake()
    {
        stingCollider = stingerGFX.GetComponent<Collider2D>();
        applyStatusEffects = ownStatusManager.ApplyStatusEffects;
    }

    void Update()
    {
        HandleDetection();
    }

    void HandleDetection()
    {
        limbTargets.Clear();

        colliderTargets = new(Physics2D.OverlapCircleAll(transform.position, stingerRadius, creatureLayer));

        limbTargets = GetLimbsInColliders(colliderTargets);

        if (limbTargets.Count > 0)
            AttackTarget(limbTargets[Random.Range(0, limbTargets.Count)]);
    }

    List<LimbSubject> GetLimbsInColliders(List<Collider2D> colliders)
    {
        List<LimbSubject> _limbSubjects = new();
        for (int i = 0; i < colliders.Count; i++)
        {
            if (!colliders[i].TryGetComponent(out LimbSubject limbTarget))
                limbTarget = colliders[i].GetComponentInParent<LimbSubject>();

            if (limbTarget == null) continue;

            if (limbTarget.OwnStatusManager == ownStatusManager) continue;

            if (!ownStatusManager.TargetLayer.HasFlag(limbTarget.OwnStatusManager.CreatureType)) continue;

            if (!_limbSubjects.Contains(limbTarget))
                _limbSubjects.Add(limbTarget);
        }

        return _limbSubjects;
    }

    public void AttackTarget(LimbSubject limbTarget)
    {
        if (cooldownCoroutine == null && attackCoroutine == null)
            attackCoroutine = StartCoroutine(Attack(limbTarget));

    }

    public IEnumerator Attack(LimbSubject limbTarget)
    {
        float attackTime = 0;
        float currentAttackWindupTime = 0;
        Transform attackTrans = limbTarget.OwnStatusManager.GrabManager.GetClosestGrabTrans(transform.position);

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

        StopCoroutineIfAssigned(hitDetectCoroutine);
        hitDetectCoroutine = StartCoroutine(DetectIfHit());

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

    IEnumerator DetectIfHit()
    {
        float detectTime = 0;
        while (attackCoroutine != null && detectTime < timeToAttack)
        {
            detectTime += Time.deltaTime;

            colliderAttack.Clear();
            Physics2D.OverlapCollider(stingCollider, contactFilter, colliderAttack);

            List<LimbSubject> _limbTargets = GetLimbsInColliders(colliderAttack);
            if (_limbTargets.Count < 0) yield return null;

            for (int i = 0; i < _limbTargets.Count; i++)
            {
                applyStatusEffects.ApplyEffects(statusEffects, _limbTargets[i], ownStatusManager);
            }

            yield return new WaitForSeconds(timeToAttack - detectTime);
        }

        cooldownCoroutine = StartCoroutine(Cooldown());
        attackCoroutine = null;
        hitDetectCoroutine = null;
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

    void StopCoroutineIfAssigned(Coroutine coroutine)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, stingerRadius);
        Gizmos.DrawLine(stingerGFX.position, stingerTarget.transform.position);
    }
}