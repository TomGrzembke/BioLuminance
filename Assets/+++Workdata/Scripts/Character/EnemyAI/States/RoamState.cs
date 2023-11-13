using UnityEngine;

public class RoamState : State
{
    public TestState d;
    //#region serialized fields

    //[Space(5)]
    //public Vector2 roamPosition;
    //public Vector3 startingPosition;
    //[SerializeField] float minRoamRange = 5f;
    //[SerializeField] float maxRoamRange = 5f;
    //[Space(5)]
    //[SerializeField] ChaseState chaseState;

    //#endregion

    //#region private fields

    //#endregion

    public override State SwitchState()
    {
        //HandleDetection();
        //HandleRoaming(enemyManager);
        //HandleRotate(enemyManager);

        ////Look for a potential target
        ////Switch to chase state if target is found

        //if (enemyManager.currentTarget != null)
        //{
        //    return chaseState;
        //}
        //else
        //{
        //    return this;
        //}
        return d;
        return this;
    }

    protected override void EnterInternal()
    {
        
    }

    protected override void UpdateInternal()
    {
    }

    protected override void FixedUpdateInternal()
    {
    }

    protected override void ExitInternal()
    {
        print("sdgunds");
    }

    //void Start()
    //{
    //    startingPosition = transform.position;
    //    roamPosition = startingPosition;
    //}

    //Vector3 GetRandomRoamingPosition()
    //{
    //    return startingPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(minRoamRange, maxRoamRange);
    //}

    //private void HandleRoaming(StateManager enemyManager)
    //{
    //    enemyManager.agent.SetDestination(roamPosition);
    //    enemyManager.enemyStoppingDistance = 0f;

    //    float reachedPositionDistance = 1f;

    //    if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
    //    {
    //        roamPosition = GetRandomRoamingPosition();
    //    }
    //}
    //private void HandleRotate(StateManager enemyManager)
    //{
    //    Vector3 velocity = enemyManager.agent.velocity;
    //    velocity.z = 0;
    //    velocity.Normalize();

    //    if (velocity != Vector3.zero)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, velocity);

    //        float step = enemyManager.enemyAcceleration * Time.deltaTime;
    //        enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
    //    }
    //}

    //public void HandleDetection()
    //{
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayer);

    //    for (int i = 0; i < colliders.Length; i++)
    //    {
    //        CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

    //        if (characterStats != null)
    //        {
    //            //It looks for a target on a certain layer, and if that target has the characterStats script, it's added to it's target list

    //            Vector2 targetDirection = (characterStats.transform.position - transform.position).normalized;

    //            if (Vector2.Angle(transform.up, targetDirection) < angle / 2)
    //            {
    //                if (!Physics2D.Raycast(transform.position, targetDirection, distanceFromTarget, obstacleLayer))
    //                {
    //                    canSeePlayer = true;
    //                    currentTarget = characterStats;
    //                }
    //                else
    //                {
    //                    canSeePlayer = false;
    //                    currentTarget = null;
    //                }
    //            }
    //            else if (canSeePlayer)
    //                canSeePlayer = false;
    //        }
    //    }
    //}
}