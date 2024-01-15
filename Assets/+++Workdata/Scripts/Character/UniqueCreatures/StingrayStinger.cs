using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingrayStinger : MonoBehaviour
{
    #region serialized fields

    [Header("Stinger Info")]

    [SerializeField] GameObject snapStingerTo;
    [SerializeField] GameObject stinger;
    [SerializeField] GameObject stingerGFX;
    [SerializeField] Transform stingerTip;
    [SerializeField] float stingerRadius;
    [Space(5)] public float stingerDamage;
    public float placeholderHealth;

    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] private float animationCurveDuration = 1;

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
        stingerOriginalPosition = stinger.transform.localPosition;
        stingerGFXOriginalPosition = stingerGFX.transform.localPosition;
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

        colliders = new List<Collider2D>(Physics2D.OverlapCircleAll(stinger.transform.position, stingerRadius,
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
            stingerGFX.transform.position = Vector3.Lerp(stingerGFX.transform.position,
                statusTargets[0].transform.position,
                Mathf.Clamp01(animationCurve.Evaluate(animationCurveDuration * Time.deltaTime)));
            StingerTipLookAt();
            yield return new WaitForSeconds(0.2f);
            ResetStinger();
            StingerTipLookAt();
            yield return new WaitForSeconds(0.2f);
            stingerGFX.transform.localPosition = stingerGFXOriginalPosition;
        }
    }

    private void StingerTipLookAt()
    {
        stingerTip.transform.LookAt(stingerGFX.transform.position);
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
        stinger.transform.localPosition = stingerOriginalPosition;
        stingerGFX.transform.localPosition = Vector3.Lerp(stingerGFX.transform.localPosition, stingerGFXOriginalPosition,
            Mathf.Clamp01(animationCurve.Evaluate(animationCurveDuration * Time.deltaTime)));

        attackTime = timeToAttack;
        isCooldown = true;
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(stinger.transform.position, stingerRadius);
        Gizmos.DrawLine(stinger.transform.position, stingerGFX.transform.position);
    }
}