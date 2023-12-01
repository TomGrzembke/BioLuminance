using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TentacleController : MonoBehaviour
{
    #region serialized fields
    [SerializeField] float detachCheckTime = 3;
    [SerializeField] TentacleTargetManager tentacleTargetManager;
    [SerializeField] PlayerDetect cursorDetect;
    [SerializeField] PlayerDetect playerDetect;
    #endregion

    #region private fields
    bool targetCursor;
    PlayerInputActions inputActions;
    [SerializeField] List<StatusManager> savedPossibleTarget;
    Coroutine checkIfStillInRangeCO;
    #endregion

    void Awake()
    {
        tentacleTargetManager = GetComponent<TentacleTargetManager>();
        inputActions = new();

        inputActions.Player.Attack.performed += ctx => Attack();
    }

    void Attack()
    {
        if (!cursorDetect.HasTargets)
        {
            targetCursor = !targetCursor;
            tentacleTargetManager.SetOneTarget(targetCursor ? cursorDetect.transform : null);
        }
        else
        {
            targetCursor = true;
            for (int i = 0; i < cursorDetect.PossibleTargets.Count; i++)
            {
                if (!savedPossibleTarget.Contains(cursorDetect.PossibleTargets[i]))
                    savedPossibleTarget.Add(cursorDetect.PossibleTargets[i]);
            }
            tentacleTargetManager.SetAttackStatusManager(cursorDetect.PossibleTargets);

            if (checkIfStillInRangeCO != null)
                StopCoroutine(checkIfStillInRangeCO);
            checkIfStillInRangeCO = StartCoroutine(CheckIfStillInRange());
        }

    }

    IEnumerator CheckIfStillInRange()
    {
        yield return new WaitForSeconds(detachCheckTime);

        for (int i = 0; i < savedPossibleTarget.Count; i++)
        {
            if (!playerDetect.PossibleTargets.Contains(savedPossibleTarget[i]))
            {
                savedPossibleTarget.RemoveAt(i);
                tentacleTargetManager.SetAttackStatusManager(savedPossibleTarget);
                i--;
            }
        }

        if (savedPossibleTarget.Count == 0)
        {
            targetCursor = false;
            tentacleTargetManager.ResetTentacles();
        }
        else
            checkIfStillInRangeCO = StartCoroutine(CheckIfStillInRange());
    }

    #region OnEnable/Disable
    public void OnEnable()
    {
        inputActions.Enable();
    }

    public void OnDisable()
    {
        inputActions.Disable();
    }
    #endregion
}