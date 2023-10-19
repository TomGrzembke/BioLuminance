using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemyAI : MonoBehaviour
{
    #region serialized fields

    [SerializeField] LayerMask detectionLayer;
    public CharacterStats currentTarget;

    #endregion

    #region private fields

    NewEnemyManager enemyManager;
    NavMeshAgent agent;

    #endregion

    private void Awake()
    {
        #region GetComponent

        enemyManager = GetComponent<NewEnemyManager>();
        agent = GetComponent<NavMeshAgent>();

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
}