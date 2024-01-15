using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingrayStinger : MonoBehaviour
{
    #region serialized fields

    [Header("Stinger Info")]

    [SerializeField] GameObject snapStingerTo;
    [SerializeField] Transform stinger;
    [SerializeField] Transform stingerTarget;
    [SerializeField] Transform stingerTip;
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

    [MinMaxRange(0, 360)][SerializeField] RangedFloat flipRange = new(0, 180);
    float angle;

    #endregion

    #region private fields

    private Vector2 stingerOriginalPosition;
    private Vector2 stingerGFXOriginalPosition;

    bool flipped;
    bool inFlipRange;
    bool isCooldown;
    [HideInInspector] public bool isAttacking;

    float attackTime;

    Coroutine attackRoutine;
    #endregion

    void Start()
    {
        stingerOriginalPosition = stinger.localPosition;
        stingerGFXOriginalPosition = stingerTarget.localPosition;
        attackTime = timeToAttack;
    }

    void Update()
    {
        HandleDetection();
        AttachStinger();
    }

    public void AttachStinger()
    {
        gameObject.transform.position = snapStingerTo.transform.position;
        gameObject.transform.rotation = snapStingerTo.transform.rotation;
    }

    public void HandleDetection()
    {
        statusTargets.Clear();

        colliders = new List<Collider2D>(Physics2D.OverlapCircleAll(stinger.position, stingerRadius,
            creatureLayer));

        for (int i = 0; i < colliders.Count; i++)
        {
            if (!colliders[i].TryGetComponent(out StatusManager statusTarget))
                statusTarget =
                    colliders[i].GetComponentInParent<StatusManager>();

            if (!statusTarget.TargetLayer.HasFlag(creatureType))
                continue;

            if (!statusTargets.Contains(statusTarget))
                statusTargets.Add(statusTarget);

            AttackTarget();
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
            stingerTarget.position = Vector3.Lerp(stingerTarget.position,
                statusTargets[0].transform.position,
                Mathf.Clamp01(animationCurve.Evaluate(animationCurveDuration * Time.deltaTime)));
            StingerTipLookAt();
            yield return new WaitForSeconds(0.2f);
            ResetStinger();
            StingerTipLookAt();
            yield return new WaitForSeconds(0.2f);
            stingerTarget.localPosition = stingerGFXOriginalPosition;
        }
    }

    private void StingerTipLookAt()
    {
        stingerTip.transform.LookAt(stingerTarget.position);
        Quaternion quaternion = stingerTip.transform.rotation;
        quaternion.y = 0;
        stingerTip.transform.rotation = quaternion;
    }

    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownForNextAttack);
        isCooldown = false;
    }

    public void ResetStinger()
    {
        stinger.localPosition = stingerOriginalPosition;
        stingerTarget.localPosition = Vector3.Lerp(stingerTarget.localPosition, stingerGFXOriginalPosition,
            Mathf.Clamp01(animationCurve.Evaluate(animationCurveDuration * Time.deltaTime)));

        attackTime = timeToAttack;
        isCooldown = true;
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(stinger.position, stingerRadius);
        Gizmos.DrawLine(stinger.position, stingerTarget.transform.position);
    }
}