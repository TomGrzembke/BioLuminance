using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public abstract class CreatureLogic : MonoBehaviour
{
    #region serialized fields
    public Health TargetHealthScript => targetHealthScript;
    [Header("AI Targeting")]
    [SerializeField] Health targetHealthScript;

    public LayerMask TargetLayer => targetLayer;
    [SerializeField] LayerMask targetLayer;

    public bool CanSeeTarget => canSeeTarget;
    [SerializeField] bool canSeeTarget = false;

    [SerializeField] float distanceFromTarget;
    public float DistanceFromTarget => distanceFromTarget;

    [Header("AI Settings")]

    [SerializeField] float detectionRadius;
    public float DetectionRadius => detectionRadius;

    public float Angle => angle;
    [Range(0, 360)][SerializeField] float angle = 50f;

    public float AgentSpeed => agentSpeed;
    [SerializeField] float agentSpeed = 3.5f;
    float defaultAgentSpeed = 3.5f;

    public float AgentAcceleration => agentAcceleration;
    [SerializeField] float agentAcceleration = 5f;
    float defaultAgentAcceleration = 3.5f;

    public float AgentStoppingDistance => agentStoppingDistance;
    [SerializeField] float agentStoppingDistance = 1f;
    float defaultAgentStoppingDistance = 3.5f;

    [SerializeField] protected StateManager stateManager;
    public LayerMask ObstacleLayer => obstacleLayer;
    [SerializeField] LayerMask obstacleLayer;

    [Header("Health")]
    [SerializeField] LimbManager limbManager;
    #endregion

    #region private fields
    protected StunState stunState;
    protected Stun stun;
    [HideInInspector] public NavMeshAgent agent;
    Health thisHealthScript;
    #endregion

    void Awake()
    {
        thisHealthScript = GetComponentInChildren<Health>();
        stunState = GetComponentInChildren<StunState>();
        stun = GetComponentInChildren<Stun>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    void Start()
    {
        defaultAgentSpeed = agentSpeed;
        defaultAgentAcceleration = agentAcceleration;
        defaultAgentStoppingDistance = agentStoppingDistance;
    }
    void OnValidate()
    {
        agent = GetComponent<NavMeshAgent>();
        RefreshAgentVars();
    }
    void OnEnable()
    {
        stun.RegisterOnStun(OnStun);
    }
    void OnDisable()
    {
        stun.OnStun -= OnStun;
    }

    public void RefreshAgentVars()
    {
        agent.speed = agentSpeed;
        agent.acceleration = agentAcceleration;
        agent.stoppingDistance = agentStoppingDistance;
    }
    public void ResetAgentVars()
    {
        agent.speed = defaultAgentSpeed;
        agent.acceleration = defaultAgentAcceleration;
        agent.stoppingDistance = defaultAgentStoppingDistance;
    }
    public void RefreshAgentVars(float newSpeed = 3, float newAcceleration = 5, float newStoppingDistance = 0)
    {
        agentSpeed = newSpeed;
        agentAcceleration = newAcceleration;
        agentStoppingDistance = newStoppingDistance;
        RefreshAgentVars();
    }

    public void HandleRotate()
    {
        Vector3 velocity = agent.velocity;
        velocity.z = 0;
        velocity.Normalize();

        if (velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, velocity);

            float step = AgentAcceleration * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        }
    }

    void OnDrawGizmosSelected()
    {
        Handles.color = Color.green;
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, DetectionRadius); //This visualizes the detection radius

        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2); //This seperates the Angle into two different values
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, angle / 2); //This seperates the Angle into two different values

        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(Vector3.zero, viewAngle01 * DetectionRadius); //This visualizes the FOV 
        Gizmos.DrawLine(Vector3.zero, viewAngle02 * DetectionRadius); //This visualizes the FOV 
    }
    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    void OnStun(bool condition)
    {
        if (condition)
        {
            stateManager.SetState(stunState);
            return;
        }

        stateManager.SetState(stateManager.LastState);
        agent.isStopped = condition;
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

    public void SetTargetHealthScript(Health newTarget)
    {
        if (newTarget != thisHealthScript)
            targetHealthScript = newTarget;
    }
    #endregion
}