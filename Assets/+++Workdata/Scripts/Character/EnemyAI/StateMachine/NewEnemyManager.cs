using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyManager : MonoBehaviour
{
    #region serialized fields

    public bool isPerformingAction;

    [Header("AI Settings")]
    public float detectionRadius = 20f;
    //Field of View
    public float maxDetectionAngle = 50f;
    public float minDetectionAngle = -50f;

    #endregion

    #region private fields

    NewEnemyAI enemyAI;

    #endregion

    private void Awake()
    {
        #region GetComponent

        enemyAI = GetComponent<NewEnemyAI>();

        #endregion
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        HandleCurrentAction();
    }

    private void HandleCurrentAction()
    {
        if (enemyAI.currentTarget == null)
        {
            enemyAI.HandleDetection();
        }
        else
        {
            enemyAI.HandleMoveToTarget();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; //replace red with whatever color you prefer
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}