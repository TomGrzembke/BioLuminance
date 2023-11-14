using MyBox;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public abstract class CreatureLogic : MonoBehaviour
{
    #region serialized fields
    public CreatureLogic currentTarget;

    [Header("AI Settings")]
    public bool canSeePlayer = false;
    public LayerMask detectionLayer;
    public LayerMask obstacleLayer;
    public float detectionRadius;
    [Range(0, 360)] public float angle = 50f;

    public float distanceFromTarget;
    public float enemySpeed = 3.5f;
    public float enemyAcceleration = 5f;
    public float enemyStoppingDistance = 1f;

    //animation reference

    [Header("Health Settings")]
    [SerializeField] EnemyLimbStats[] enemyLimbStats;

    #endregion

    #region private fields
    static event Action<CreatureLogic> OnEnemyDied;

    [HideInInspector]public NavMeshAgent agent;
    #endregion
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
     void OnValidate()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemySpeed;
        agent.acceleration = enemyAcceleration;
        agent.stoppingDistance = enemyStoppingDistance;
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

    [ButtonMethod()]
    public int CollectLimbs()
    {
        enemyLimbStats = GetComponentsInChildren<EnemyLimbStats>();
        return enemyLimbStats.Length;
    }
}