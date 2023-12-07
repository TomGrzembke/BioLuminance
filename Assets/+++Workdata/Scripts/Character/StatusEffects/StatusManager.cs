using UnityEngine;

public class StatusManager : MonoBehaviour
{
    #region serialized fields
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

    public GrabTransformManager GrabManager => grabManager;
    [SerializeField] GrabTransformManager grabManager;
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
        stunSubject.AddStun(additionalStun);
    }

    public void ApplyTentacle(TentacleDetection.TentacleEffects tentacleEffects)
    {
        AddDamage(tentacleEffects.damagePerInstance);
        AddStun(tentacleEffects.stunPerInstance);
    }
}