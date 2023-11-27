using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public abstract class CreatureLogic : MonoBehaviour
{
    #region serialized fields
    public StatusManager TargetStatusManager => targetStatusManager;
    [Header("AI Targeting")]
    [SerializeField] StatusManager targetStatusManager;

    public LayerMask TargetLayer => targetLayer;
    [SerializeField] LayerMask targetLayer;

    public bool CanSeeTarget => canSeeTarget;

    [SerializeField] float distanceFromTarget;
    public float DistanceFromTarget => distanceFromTarget;

    [Header("AI Settings")]
    [SerializeField] float defaultAgentSpeed = 3.5f;

    [SerializeField] float defaultAgentAcceleration = 3.5f;

    [SerializeField] float defaultAgentStoppingDistance = 3.5f;

    [SerializeField] float detectionRadius;
    public float DetectionRadius => detectionRadius;

    public float DetectionAngle => detectionAngle;
    [Range(0, 360)][SerializeField] float detectionAngle = 50f;
    public LayerMask ObstacleLayer => obstacleLayer;
    [SerializeField] LayerMask obstacleLayer;

 
    #endregion

    #region private fields

    bool canSeeTarget = false;
    protected StunState stunState;
    protected ChaseState chaseState;
    protected FleeState fleeState;
    protected DeathState deathState;
    protected StunSubject stun;
    [HideInInspector] public NavMeshAgent agent;
    HealthSubject healthSubject;
    SpeedSubject speedSubject;
    #endregion

    void Awake()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Start()
    {
        ResetAgentVars();
    }

    void OnValidate()
    {
        fleeState = GetComponentInChildren<FleeState>();
        deathState = GetComponentInChildren<DeathState>();
        healthSubject = GetComponentInChildren<HealthSubject>();
        speedSubject = GetComponentInChildren<SpeedSubject>();
        stunState = GetComponentInChildren<StunState>();
        chaseState = GetComponentInChildren<ChaseState>();
        stun = GetComponentInChildren<StunSubject>();
        agent = GetComponent<NavMeshAgent>();

        ResetAgentVars();
    }

    public void ResetAgentVars()
    {
        speedSubject.SetDefault(defaultAgentSpeed);
        agent.acceleration = defaultAgentAcceleration;
        agent.stoppingDistance = defaultAgentStoppingDistance;
    }

    public void SetAgentVars(float newSpeed = -1, float newAcceleration = -1, float newStoppingDistance = -1)
    {
        if (newSpeed != -1)
            speedSubject.SetDefault(newSpeed);
        if (newAcceleration != -1)
            agent.acceleration = newAcceleration;
        if (newStoppingDistance != -1)
            agent.stoppingDistance = newStoppingDistance;
    }

    public void HandleRotate()
    {
        Vector3 velocity = agent.velocity;
        velocity.WithoutZ();
        velocity.Normalize();

        if (velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, velocity);

            float step = agent.acceleration * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Handles.color = Color.green;
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, DetectionRadius); //This visualizes the detection radius

        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -detectionAngle / 2); //This seperates the Angle into two different values
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, detectionAngle / 2); //This seperates the Angle into two different values

        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(Vector3.zero, viewAngle01 * DetectionRadius); //This visualizes the FOV 
        Gizmos.DrawLine(Vector3.zero, viewAngle02 * DetectionRadius); //This visualizes the FOV 
    }
#endif

    Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    #region Setter
    public void SetDistanceFromTarget(float newDistance)
    {
        distanceFromTarget = newDistance;
    }

    public void SetCanSeePlayer(bool condition)
    {
        canSeeTarget = condition;
    }

    public void SetTargetStatusManager(StatusManager newTarget)
    {
        if (newTarget != targetStatusManager)
            targetStatusManager = newTarget;
    }
    #endregion
}