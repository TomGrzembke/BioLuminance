using System.Collections.Generic;
using UnityEngine;

public class ConditionModifier : MonoBehaviour
{
    #region serialized fields
    [SerializeField] DepthTracker depthTracker;
    [SerializeField] List<Condition> conditions;
    #endregion

    #region private fields

    #endregion

    void Update()
    {
        SetConditions();
    }

    void SetConditions()
    {
        if (!depthTracker)
        {
            Debug.Log(nameof(DepthTracker) + "isnt assgined", gameObject);
            return;
        }

        for (int i = 0; i < conditions.Count; i++)
        {
            if (conditions[i] != null)
                conditions[i].SetPercentageDebuffAlpha(depthTracker.Alpha);
        }
    }
}