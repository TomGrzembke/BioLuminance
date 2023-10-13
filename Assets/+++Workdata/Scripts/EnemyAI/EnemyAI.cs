using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region serialized fields

    [SerializeField] Vector2 roamPosition;
    [SerializeField] float minRoamRange = 5f;
    [SerializeField] float maxRoamRange = 5f;

    #endregion

    #region private fields

    Vector3 startingPosition;
    EnemyMovement enemyMovement;

    #endregion

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    void Start()
    {
        startingPosition = transform.position;
        roamPosition = GetRandomRoamingPosition();
    }

    Vector3 GetRandomRoamingPosition()
    {
        return startingPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(minRoamRange, maxRoamRange);
    }

    private void Update()
    {
        enemyMovement.Roam(roamPosition);

        float reachedPositionDistance = 1f;
        if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
        {
            roamPosition = GetRandomRoamingPosition();
        }
    }
}