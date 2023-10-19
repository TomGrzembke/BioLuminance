using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemyAI : MonoBehaviour
{
    #region serialized fields

    [SerializeField] LayerMask detectionLayer;
    public CharacterStats currentTarget;
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

    NewEnemyManager enemyManager;
    NewEnemyAnimationManager enemyAnim;
    NavMeshAgent agent;
    Rigidbody2D rb;

    #endregion

    void Update()
    {
        agent.speed = enemySpeed;
        agent.acceleration = enemyAcceleration;
        agent.stoppingDistance = enemyStoppingDistance;
    }

    void Awake()
    {
        #region GetComponent

        enemyManager = GetComponent<NewEnemyManager>();
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<NewEnemyAnimationManager>();
        rb = GetComponent<Rigidbody2D>();

        #endregion

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void HandleDetection()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                //If looks for a target on a certain layer, and if that target has the characterStats script, it's added to it's target list

                Vector2 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector2.Angle(targetDirection, transform.up);

                if (viewableAngle > enemyManager.minDetectionAngle && viewableAngle < enemyManager.maxDetectionAngle)
                {
                    currentTarget = characterStats;
                }
            }
        }
    }

    public void HandleMoveToTarget()
    {
        agent.SetDestination(currentTarget.transform.position);

        HandleRotateTowardsTarget();
    }

    private void HandleRotateTowardsTarget()
    {
        Vector3 velocity = agent.velocity;
        velocity.z = 0;

        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, velocity);
        }
    }
}