using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    #region serialized fields

    EnemyManager enemyManager;
    public float enemySpeed;
    public float enemyAcceleration;
    public float enemyStoppingDistance;

    #endregion

    #region private fields

    NavMeshAgent agent;

    #endregion

    private void Update()
    {
        agent.speed = enemySpeed;
        agent.acceleration = enemyAcceleration;
        agent.stoppingDistance = enemyStoppingDistance;

        transform.LookAt(agent.transform.position);
    }

    void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void MoveTo(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
        Rotate();
    }

    void Rotate()
    {
        var vel = agent.velocity;
        vel.z = 0;

        if (vel != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, vel);
        }
    }
}