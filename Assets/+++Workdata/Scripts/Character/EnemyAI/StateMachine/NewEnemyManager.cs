using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemyManager : MonoBehaviour
{
    #region serialized fields

    public bool isPerformingAction;
    public State currentState;
    public CharacterStats currentTarget;

    [Space(5)]
    public float enemyInterestCountdown;

    [Header("AI Settings")]
    [SerializeField] bool canSeePlayer = false;
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

    NewEnemyAI enemyAI;
    NewEnemyAnimationManager enemyAnimationManager;
    EnemyStats enemyStats;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Rigidbody2D rb;
    
    #endregion

    void Awake()
    {
        #region GetComponent

        enemyAI = GetComponent<NewEnemyAI>();
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
            State nextState = currentState.Tick(this, enemyAI, enemyAnimationManager, enemyStats);

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
        distanceFromTarget = 0;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                //It looks for a target on a certain layer, and if that target has the characterStats script, it's added to it's target list

                Vector2 targetDirection = characterStats.transform.position - transform.position;

                if (Vector2.Angle(transform.up, targetDirection) < angle / 2)
                {
                    currentTarget = characterStats;
                }
            }
        }
    }
    
    public void NewHandleDetection(NewEnemyManager enemyManager)
    {
        enemyManager.distanceFromTarget = 0;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();
            Vector2 targetDirection = characterStats.transform.position - transform.position;
            
            if (characterStats != null)
            {
                if (Vector2.Angle(transform.position, targetDirection) < angle / 2)
                {
                    if (!Physics.Raycast(transform.position, targetDirection, enemyManager.distanceFromTarget, obstacleLayer))
                    {
                        canSeePlayer = true;
                        enemyManager.currentTarget = characterStats;
                    }
                    else
                    {
                        canSeePlayer = false;
                        enemyManager.currentTarget = null;
                    }
                }
                else if (canSeePlayer)
                {
                    canSeePlayer = false;
                    enemyManager.currentTarget = null;
                }
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, detectionRadius);
        
        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, angle / 2);
        
        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(Vector3.zero, viewAngle01 * detectionRadius);
        Gizmos.DrawLine(Vector3.zero, viewAngle02 * detectionRadius);
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}