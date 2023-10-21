using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyManager : MonoBehaviour
{
    #region serialized fields

    public bool isPerformingAction;

    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;

    [Header("AI Settings")]
    public float detectionRadius = 20f;
    //Field of View
    public float maxDetectionAngle = 50f;
    public float minDetectionAngle = -50f;

    public float currentRecoveryTime = 0f;
    [Space(10)]
    public float pathUpdateDelay = 0.2f;
    public float pathUpdateDeadline;

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
        pathUpdateDeadline -= Time.deltaTime;
        if (pathUpdateDeadline <= 0) pathUpdateDeadline = 0;

        HandleRecoveryTime();
    }

    void FixedUpdate()
    {
        HandleCurrentAction();
    }

    private void HandleCurrentAction()
    {
        if (enemyAI.currentTarget != null)
        {
            enemyAI.distanceFromTarget = Vector3.Distance(enemyAI.currentTarget.transform.position, transform.position);
        }
        
        if (enemyAI.currentTarget == null)
        {
            enemyAI.HandleDetection();
        }
        else if (enemyAI.distanceFromTarget > enemyAI.enemyStoppingDistance)
        {
            enemyAI.HandleMoveToTarget();
        }
        else if (enemyAI.distanceFromTarget <= enemyAI.enemyStoppingDistance)
        {
            AttackTarget();
        }
    }

    private void HandleRecoveryTime()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }

    void AttackTarget()
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; //replace red with whatever color you prefer
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}