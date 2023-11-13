using MyBox;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public abstract class CreatureLogic : MonoBehaviour
{
    #region serialized fields
    [SerializeField] List<CreatureLogic> currentTargets;

    [Header("AI Settings")]
    [SerializeField] bool canSeePlayer = false;
    [SerializeField] LayerMask detectionLayer;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float detectionRadius;
    [SerializeField, Range(0, 360)] float fovAngle = 50f;

    [SerializeField] float distanceFromTarget;
    [SerializeField] float enemySpeed = 3.5f;
    [SerializeField] float enemyAcceleration = 5f;
    [SerializeField] float enemyStoppingDistance = 1f;

    //animation reference

    [Header("Health Settings")]

    [SerializeField] bool hasMultipleDamagePoints;
    [SerializeField] EnemyLimbStats[] enemyLimbStats;

    #endregion

    #region private fields
    static event Action<CreatureLogic> OnEnemyDied;

    NavMeshAgent agent;
    #endregion
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.green;
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, detectionRadius); //This visualizes the detection radius

        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -fovAngle / 2); //This seperates the Angle into two different values
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, fovAngle / 2); //This seperates the Angle into two different values

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

    [ButtonMethod(ButtonMethodDrawOrder.AfterInspector, nameof(hasMultipleDamagePoints))]
    string CollectLimbs()
    {
        enemyLimbStats = FindObjectsOfType<EnemyLimbStats>();
        return enemyLimbStats.Length + "";
    }
}