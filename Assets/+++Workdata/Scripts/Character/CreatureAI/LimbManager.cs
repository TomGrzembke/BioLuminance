using MyBox;
using System.Collections.Generic;
using UnityEngine;

public class LimbManager : MonoBehaviour
{
    #region serialized fields
    [SerializeField] List<LimbStats> enemyLimbStats = new();
    public int LimbAmount => enemyLimbStats.Count;
    #endregion

    #region private fields

    #endregion

    [ButtonMethod()]
    public int CollectLimbs()
    {
        LimbStats[] gatheredLimbs = GetComponentsInChildren<LimbStats>();

        for (int i = 0; i < enemyLimbStats.Count;)
        {
            enemyLimbStats.Add(gatheredLimbs[i]);
        }

        return enemyLimbStats.Count;
    }
}