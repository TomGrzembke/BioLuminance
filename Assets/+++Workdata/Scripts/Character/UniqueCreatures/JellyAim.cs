using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyAim : MonoBehaviour
{
    #region serialized fields
    [SerializeField] float detachCheckTime = 3;
    [SerializeField] TentacleTargetManager tentacleTargetManager;
    [SerializeField] PlayerDetect jellyDetect;
    #endregion

    #region private fields
    [SerializeField] List<LimbSubject> savedPossibleTarget;
    Coroutine checkIfStillInRangeCO;
    #endregion

    public void AttackNear()
    {
        if (!jellyDetect.HasTargets) return;

        for (int i = 0; i < jellyDetect.PossibleTargets.Count; i++)
        {
            if (!savedPossibleTarget.Contains(jellyDetect.PossibleTargets[i]))
                savedPossibleTarget.Add(jellyDetect.PossibleTargets[i]);
        }
        tentacleTargetManager.SetAttackStatusManager(jellyDetect.PossibleTargets);

        if (checkIfStillInRangeCO != null)
            StopCoroutine(checkIfStillInRangeCO);
        checkIfStillInRangeCO = StartCoroutine(CheckIfStillInRange());
    }

    IEnumerator CheckIfStillInRange()
    {
        yield return new WaitForSeconds(detachCheckTime);

        for (int i = 0; i < savedPossibleTarget.Count; i++)
        {
            if (!jellyDetect.PossibleTargets.Contains(savedPossibleTarget[i]))
            {
                savedPossibleTarget.RemoveAt(i);
                i--;
            }
            tentacleTargetManager.SetAttackStatusManager(savedPossibleTarget);
        }

        if (savedPossibleTarget.Count == 0)
        {
            tentacleTargetManager.ResetTentacles();
        }
        else
            checkIfStillInRangeCO = StartCoroutine(CheckIfStillInRange());
    }
}
