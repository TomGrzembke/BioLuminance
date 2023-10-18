using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class EnemyManager : MonoBehaviour
{
    #region serialized fields

    [Header("General Information")]
    [SerializeField] OldState oldState;
    public float enemyHP;

    [Foldout("Roam Information", true)]
    public Vector2 roamPosition;
    [SerializeField] float minRoamRange = 5f;
    [SerializeField] float maxRoamRange = 5f;

    [Foldout("Chase Information", true)]
    [SerializeField] float minDetectionRange = 10f;
    [SerializeField] float maxDetectionRange = 15f;
    [SerializeField] float attackRange = 5f;

    [Foldout("Attack Information", true)]
    [SerializeField] float placeholder2;

    [Foldout("Stalk Information", true)]
    [SerializeField] float placeholder3;

    #endregion

    #region private fields

    Vector3 startingPosition;
    EnemyMovement enemyMovement;
    EnemyAnimationController enemyAnim;
    PlayerController playerController;
    SpriteRenderer spriteRenderer;

    #endregion

    #region enums/OnValidate

    public enum OldState
    {
        Roaming,
        ChaseTarget,
        AttackTarget,
    }

    #endregion

    private void Awake()
    {
        oldState = OldState.Roaming;

        #region GetComponent

        enemyMovement = GetComponent<EnemyMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnim = GetComponent<EnemyAnimationController>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            playerController = playerObject.GetComponent<PlayerController>();
        else
            Debug.LogError("Player GameObject not found.");

        #endregion
    }

    void Start()
    {
        startingPosition = transform.position;
        roamPosition = GetRandomRoamingPosition();
        oldState = OldState.Roaming;
    }

    void Update()
    {
        switch (oldState)
        {
            default:
            case OldState.Roaming:

                Roam();
                FindTarget();
                enemyAnim.StopAttackAnim();
                break;

            case OldState.ChaseTarget:

                Chase();
                FindTarget();
                enemyAnim.StopAttackAnim();
                break;

            case OldState.AttackTarget:
                Attack();
                FindTarget();
                break;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxDetectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minDetectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    Vector3 GetRandomRoamingPosition()
    {
        return startingPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(minRoamRange, maxRoamRange);
    }

    #region States

    public void FindTarget()
    {
        if (Vector3.Distance(transform.position, playerController.playerPosition) < attackRange)
        {
            oldState = OldState.AttackTarget;
        }

        else if (Vector3.Distance(transform.position, playerController.playerPosition) < maxDetectionRange)
        {
            oldState = OldState.ChaseTarget;

            enemyMovement.enemySpeed = 2f;
            enemyMovement.enemyStoppingDistance = 1f;

            if (Vector3.Distance(transform.position, playerController.playerPosition) < minDetectionRange)
            {
                enemyMovement.enemySpeed = 3.5f;
            }
        }

        else
        {
            oldState = OldState.Roaming;
        }
    }

    void Roam()
    {
        enemyMovement.MoveTo(roamPosition);
        enemyMovement.enemyStoppingDistance = 0f;

        float reachedPositionDistance = 1f;
        if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
        {
            roamPosition = GetRandomRoamingPosition();
        }
    }

    void Chase()
    {
        enemyMovement.MoveTo(playerController.playerPosition);
        enemyMovement.enemyStoppingDistance = 1.5f;
    }


    void Attack()
    {
        enemyAnim.PlayAttackAnim();
        Chase();
    }
    #endregion
}