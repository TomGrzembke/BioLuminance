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

    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] float animationCurveDuration = 1;

    [Header("Time Info")][SerializeField] float timeToAttack;
    [SerializeField] float cooldownForNextAttack;

    [Header("Layer Info")] public LayerMask creatureLayer;
    [SerializeField] Creatures creatureType;
    [SerializeField] Creatures targetLayer;

    [SerializeField] List<StatusManager> statusTargets;
    List<Collider2D> colliders;

    #endregion

    #region private fields

    Coroutine attackCoroutine;
    Coroutine cooldownCoroutine;
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
        if (cooldownCoroutine == null)
            attackCoroutine = StartCoroutine(Attack(statusTarget));
    }

    public IEnumerator Attack(StatusManager statusTarget)
    {
        float attackTime = 0;
        Vector3 currentPos = stingerTarget.position;
        Vector3 attackPos = statusTarget.GrabManager.GetClosestGrabTrans(transform.position).position;

        while (attackTime < timeToAttack)
        {
            attackTime += Time.deltaTime;
            stingerTarget.position = Vector3.Lerp(currentPos, attackPos, attackTime / timeToAttack);
            yield return null;
        }

        cooldownCoroutine = StartCoroutine(Cooldown());
        attackCoroutine = null;
    }

    private void StingerTipLookAt()
    {
        stingerGFX.transform.LookAt(stingerTarget.position * 2);
        Quaternion quaternion = stingerGFX.transform.rotation;
        quaternion.y = 0;
        stingerGFX.transform.rotation = quaternion;
    }

    public IEnumerator Cooldown()
    {
        float resetTime = 0;
        Vector3 currentPos = stingerTarget.localPosition;

        while (resetTime < cooldownForNextAttack)
        {
            resetTime += Time.deltaTime;
            stingerTarget.localPosition = Vector3.Lerp(currentPos, Vector2.zero, resetTime / cooldownForNextAttack);
            yield return null;
        }

        yield return new WaitForSeconds(cooldownForNextAttack - resetTime);

        cooldownCoroutine = null;
    }

    public void ResetStinger()
    {
        stingerTarget.localPosition = Vector3.Lerp(stingerTarget.localPosition, Vector2.zero,
        Mathf.Clamp01(animationCurve.Evaluate(animationCurveDuration * Time.deltaTime)));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(stingerGFX.position, stingerRadius);
        Gizmos.DrawLine(stingerGFX.position, stingerTarget.transform.position);
    }
}