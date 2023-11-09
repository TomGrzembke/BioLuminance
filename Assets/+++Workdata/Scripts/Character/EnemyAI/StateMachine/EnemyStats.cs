using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class EnemyStats : CharacterStats
{
    #region serialized fields

    [Header("Health Settings")]
    
    [SerializeField] bool hasMultipleDamagePoints;
    [SerializeField] EnemyLimbStats[] enemyLimbStats;

    [ButtonMethod(ButtonMethodDrawOrder.AfterInspector, nameof(hasMultipleDamagePoints))]
    string CollectLimbs()
    {
        enemyLimbStats = FindObjectsOfType<EnemyLimbStats>();
        return enemyLimbStats.Length + "";
    }

    #endregion

    #region private fields

    #endregion
}