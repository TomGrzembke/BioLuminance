using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class NewEnemyManager : MonoBehaviour
{
    #region serialized fields

    public bool isPerformingAction;
    public State currentState;
    public CharacterStats currentTarget;

    [Space(5)]
    public float enemyInterestCountdown;

    [Header("AI Settings")]
    public bool canSeePlayer = false;
    [SerializeField] LayerMask detectionLayer;
    [SerializeField] LayerMask obstacleLayer;
    public float detectionRadius;
    //Field of View
    [SerializeField, Range(0, 360)] float angle = 50f;

    [Space(10)]
    public float distanceFromTarget;
    [Tooltip("Controls the speed of the agent.")]
    public float enemySpeed = 3.5f;
    [Tooltip("Controls the rotation speed of the agent. The smaller the number, the slower it turns.")]
    public float enemyAcceleration = 5f;
    [Tooltip("Stops before Navmesh Obstacles or agents. The higher the number, the further away it stops.")]
    public float enemyStoppingDistance = 1f;

    #endregion

    #region private fields
    
    NewEnemyAnimationManager enemyAnimationManager;
    EnemyStats enemyStats;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Rigidbody2D rb;
    
    #endregion

    void Awake()
    {
        #region GetComponent
        
        enemyAnimationManager = GetComponent<NewEnemyAnimationManager>();
        enemyStats = GetComponent<EnemyStats>();

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();

        #endregion

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        agent.speed = enemySpeed;
        agent.acceleration = enemyAcceleration;
        agent.stoppingDistance = enemyStoppingDistance;
    }

    void FixedUpdate()
    {
        HandleStateMachine();
    }

    void HandleStateMachine()
    {
        if (currentState != null)
        {
            State nextState = currentState.Tick(this, enemyAnimationManager, enemyStats);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }
    
    void SwitchToNextState(State state)
    {
        currentState = state;
    }

    public void HandleDetection()
    {
        //distanceFromTarget = 0;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                //It looks for a target on a certain layer, and if that target has the characterStats script, it's added to it's target list

                Vector2 targetDirection = (characterStats.transform.position - transform.position).normalized;

                if (Vector2.Angle(transform.up, targetDirection) < angle / 2)
                {
                    if (!Physics2D.Raycast(transform.position, targetDirection, distanceFromTarget, obstacleLayer))
                    {
                        canSeePlayer = true;
                        currentTarget = characterStats;
                    }
                    else
                    {
                        canSeePlayer = false;
                        currentTarget = null;
                    }
                }
                else if (canSeePlayer)
                    canSeePlayer = false;
            }
        }
    }

    public void GetHealth(float health)
    {
        
    }
    
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.green;
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, detectionRadius); //This visualizes the detection radius
        
        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2); //This seperates the Angle into two different values
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, angle / 2); //This seperates the Angle into two different values
        
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(Vector3.zero, viewAngle01 * detectionRadius); //This visualizes the FOV 
        Gizmos.DrawLine(Vector3.zero, viewAngle02 * detectionRadius); //This visualizes the FOV 
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}