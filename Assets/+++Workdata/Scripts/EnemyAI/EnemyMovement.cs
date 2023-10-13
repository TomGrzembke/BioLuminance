using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    #region serialized fields

    [SerializeField] Transform target;
    [SerializeField] bool shouldChase;

    #endregion

    #region private fields

    NavMeshAgent agent;

    #endregion

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        /*
        if (shouldChase)
            Chase();
        else
            Roam();
        */
    }

    public void Chase()
    {
        agent.SetDestination(target.position);
        print("Chase");
    }

    public void Roam(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
        print("Roaming");
    }
}