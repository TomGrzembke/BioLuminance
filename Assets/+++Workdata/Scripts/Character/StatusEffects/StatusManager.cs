using MyBox;
using UnityEngine;
using UnityEngine.AI;

public class StatusManager : MonoBehaviour
{
    #region serialized fields
    [SerializeField] bool isPlayer;
    public bool IsPlayer => isPlayer;
    public Creatures TargetLayer => targetLayer;
    [SerializeField] Creatures targetLayer;
    public Creatures CreatureType => creatureType;
    [SerializeField] Creatures creatureType;
    public Transform Trans => trans;
    [SerializeField] Transform trans;
    public StunSubject StunSunject => stunSubject;
    [SerializeField] StunSubject stunSubject;
    public HealthSubject HealthSubject => healthSubject;
    [SerializeField] HealthSubject healthSubject;
    public SpeedSubject SpeedSubject => speedSubject;
    [SerializeField] SpeedSubject speedSubject;
    public GrabTransManager GrabManager => grabManager;
    [SerializeField] GrabTransManager grabManager;
    public ApplyStatusEffects ApplyStatusEffects => applyStatusEffects;
    [SerializeField] ApplyStatusEffects applyStatusEffects;
    public CreatureRewards CreatureRewards => creatureRewards;
    [SerializeField, ConditionalField(nameof(isPlayer), true)] CreatureRewards creatureRewards;
    public PointSubject PointSubject => pointSubject;
    [SerializeField] PointSubject pointSubject;

    [SerializeField, ConditionalField(nameof(isPlayer))] NavMeshAgent agent;
    public NavMeshAgent Agent => agent;
    #endregion

    #region private fields

    #endregion

    void Awake()
    {
        trans = transform;
    }

    public void AddHealth(float additionalHealth)
    {
        healthSubject.AddHealth(additionalHealth);
    }
    public void AddDamage(float additionalHealth)
    {
        healthSubject.AddHealth(-additionalHealth);
    }

    public void AddStun(float additionalStun)
    {
        if (Time.timeScale > 0)
            stunSubject.AddStun(additionalStun);
    }

    public void AddSpeedModifier(SpeedModifier speedModifier)
    {
        speedSubject.AddSpeedModifier(speedModifier);
    }

    public void RemoveSpeedModifier(SpeedModifier speedModifier)
    {
        speedSubject.RemoveSpeedModifier(speedModifier);
    }
}