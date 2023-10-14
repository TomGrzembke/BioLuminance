using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region serialized fields

    [SerializeField] Vector2 roamPosition;
    [SerializeField] float minRoamRange = 5f;
    [SerializeField] float maxRoamRange = 5f;
    [SerializeField] float targetDetectionRange = 10f;

    #endregion

    #region private fields

    Vector3 startingPosition;
    EnemyMovement enemyMovement;
    PlayerController playerController;
    SpriteRenderer spriteRenderer;
    public State state;

    #endregion

    #region enums/OnValidate

    public enum State
    {
        Roaming,
        ChaseTarget,
    }

    #endregion

    private void Awake()
    {
        state = State.Roaming;

        #region GetComponent

        enemyMovement = GetComponent<EnemyMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
    }
    void Update()
    {
        switch (state)
        {
            default:
            case State.Roaming:

                Roam();
                FindTarget();
                break;

            case State.ChaseTarget:

                Chase();
                FindTarget();
                break;
        }
    }

    Vector3 GetRandomRoamingPosition()
    {
        return startingPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(minRoamRange, maxRoamRange);
    }

    void FindTarget()
    {
        if (Vector3.Distance(transform.position, playerController.playerPosition) < targetDetectionRange)
        {
            state = State.ChaseTarget;
        }
        else
        {
            state = State.Roaming;
        }
    }

    void Roam()
    {
        enemyMovement.MoveTo(roamPosition);

        float reachedPositionDistance = 1f;
        if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
        {
            roamPosition = GetRandomRoamingPosition();
        }
    }

    void Chase()
    {
        enemyMovement.MoveTo(playerController.playerPosition);
    }
}