using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingrayStinger : MonoBehaviour
{
    #region serialized fields

    [Header("Stinger Info")]

    [SerializeField] Transform stingerGFX;
    [SerializeField] Transform stingerTarget;
    [SerializeField] float stingerRadius;
    [SerializeField] FlipSpriteOnAngle stingFlipper;

    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] float animationCurveDuration = 1;

    [Header("Time Info")][SerializeField] float timeToAttack;
    [SerializeField] float cooldownForNextAttack;
    [SerializeField] float attackWindupTime = .7f;

    [Header("Layer Info")] public LayerMask creatureLayer;
    [SerializeField] Creatures creatureType;
    [SerializeField] Creatures targetLayer;

    [SerializeField] List<StatusManager> statusTargets;
    List<Collider2D> colliders;

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

        colliders = new(Physics2D.OverlapCircleAll(stingerGFX.position, stingerRadius, creatureLayer));

        for (int i = 0; i < colliders.Count; i++)
        {
            if (!colliders[i].TryGetComponent(out StatusManager statusTarget))
                statusTarget = colliders[i].GetComponentInParent<StatusManager>();

            if (!statusTarget.TargetLayer.HasFlag(creatureType))
                continue;

            if (!statusTargets.Contains(statusTarget))
                statusTargets.Add(statusTarget);

            AttackTarget(statusTarget);
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
        Vector3 attackPos = statusTarget.GrabManager.GetClosestGrabTrans(transform.position).position;
        StingerTipLookAt(attackPos);
        yield return new WaitForSeconds(attackWindupTime);

        while (attackTime < timeToAttack)
        {
            attackTime += Time.deltaTime;
            stingerTarget.position = attackPos;
            yield return null;
        }

        cooldownCoroutine = StartCoroutine(Cooldown());
        attackCoroutine = null;
    }

    void StingerTipLookAt(Vector3 pos)
    {
        Vector3 vectorToTarget = pos - stingerGFX.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - (stingFlipper.Flipped ? rotationMinus : 0);
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        stingerGFX.rotation = q;
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