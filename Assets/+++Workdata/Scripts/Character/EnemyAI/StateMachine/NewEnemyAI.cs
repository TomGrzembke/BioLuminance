using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemyAI : MonoBehaviour
{
    #region serialized fields



    #endregion

    #region private fields

    NewEnemyManager enemyManager;
    NewEnemyAnimationManager enemyAnim;

    #endregion

    void Awake()
    {
        #region GetComponent

        enemyManager = GetComponent<NewEnemyManager>();
        //agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<NewEnemyAnimationManager>();
        //rb = GetComponent<Rigidbody2D>();

        #endregion
    }
}