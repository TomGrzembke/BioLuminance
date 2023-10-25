using System.Collections;
using System.Collections.Generic;
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
    public float detectionRadius;
    //Field of View
    public float maxDetectionAngle = 50f;
    public float minDetectionAngle = -50f;
    public float angle;

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

    RoamState roamState;
    ChaseState chaseState;

    #endregion

    void Awake()
    {
        #region GetComponent

        enemyAI = GetComponent<NewEnemyAI>();
        enemyAnimationManager = GetComponent<NewEnemyAnimationManager>();
        enemyStats = GetComponent<EnemyStats>();

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();

        roamState = GetComponent<RoamState>();
        chaseState = GetComponent<ChaseState>();

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

    #region Attack (currently not using)
    /*void AttackTarget()
    {
        if (isPerformingAction)
            return;

        if (currentAttack == null)
        {
            GetNewAttack();
        }
        else
        {
            isPerformingAction = true;
            currentRecoveryTime = currentAttack.recoveryTime;
            print("Attacked");
            currentAttack = null;
        }
    }

    void GetNewAttack()
    {
        Vector3 targetDirection = enemyAI.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        enemyAI.distanceFromTarget = Vector3.Distance(enemyAI.currentTarget.transform.position, transform.position);

        int maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (enemyAI.distanceFromTarget <= enemyAttackAction.maxDistanceNeededToAttack && enemyAI.distanceFromTarget >= enemyAttackAction.minDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maxAttackAngle && viewableAngle >= enemyAttackAction.minAttackAngle)
                {
                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int tempScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (enemyAI.distanceFromTarget <= enemyAttackAction.maxDistanceNeededToAttack
                && enemyAI.distanceFromTarget >= enemyAttackAction.minDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maxAttackAngle && viewableAngle >= enemyAttackAction.minAttackAngle)
                {
                    if (currentAttack != null)
                    {
                        return;

                        tempScore += enemyAttackAction.attackScore;

                        if (tempScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
    }
    */
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; //replace red with whatever color you prefer
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}