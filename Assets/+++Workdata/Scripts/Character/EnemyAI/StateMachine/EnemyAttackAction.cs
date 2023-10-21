using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Enemy Actions/Attack Action")]
public class EnemyAttackAction : EnemyAction
{
    #region serialized fields

    public int attackScore = 3;
    public float recoveryTime = 2;

    public float maxAttackAngle = 35f;
    public float minAttackAngle = -35f;

    public float minDistanceNeededToAttack = 0f;
    public float maxDistanceNeededToAttack = 3f;

    #endregion

    #region private fields
    
    #endregion

}