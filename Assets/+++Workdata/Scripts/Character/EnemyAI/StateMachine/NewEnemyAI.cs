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
        if (enemyManager.isPerformingAction)
            return;

        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.position);

        if (enemyManager.isPerformingAction)
        {
            agent.isStopped = true;
            print("performs action");
        }
        else
        {
            if (distanceFromTarget > enemyStoppingDistance)
            {
                agent.isStopped = false;
            }
            //This is called when the target is close
            else if (distanceFromTarget <= enemyStoppingDistance)
            {
                agent.isStopped = true;
                print("Attack");
            }
        }

        HandleRotateTowardsTarget();
    }

    private void HandleRotateTowardsTarget()
    {
        //Rotate manually
        if (enemyManager.isPerformingAction || distanceFromTarget <= enemyStoppingDistance)
        {
            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.z = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.up;
            }

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);

            float step = enemyAcceleration * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        }
        
        //Rotate with Navmesh
        else
        {
            Vector3 velocity = agent.velocity;
            velocity.z = 0;
            velocity.Normalize();

            //Updates the Path every 0.2 seconds
            if (Time.deltaTime >= enemyManager.pathUpdateDeadline)
            {
                print("Updating Path");
                enemyManager.pathUpdateDeadline = Time.deltaTime + enemyManager.pathUpdateDelay;
                agent.SetDestination(currentTarget.transform.position);
            }

            if (velocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, velocity);

                float step = enemyAcceleration * Time.deltaTime;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
            }
        }
    }
}