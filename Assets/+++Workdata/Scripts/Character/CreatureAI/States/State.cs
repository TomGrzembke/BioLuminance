using System;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    #region private fields
    [Header(nameof(State))]
    [SerializeField] protected State uniqueState;

    protected CreatureLogic creatureLogic;
    public float StateAgentSpeed => stateAgentSpeed;
    [SerializeField] protected float stateAgentSpeed = 3.5f;

    public float StateAgentAcceleration => stateAgentAcceleration;
    [SerializeField] protected float stateAgentAcceleration = 5f;

    public float StateAgentStoppingDistance => stateAgentStoppingDistance;
    [SerializeField] protected float stateAgentStoppingDistance = 1f;
    [SerializeField] bool useStateVars;

    [SerializeField] StateEvent stateEvent;

    #endregion

    void Awake() => creatureLogic = GetComponentInParent<CreatureLogic>();

    [Serializable]
    public struct StateEvent
    {
        public State switchState;
        public int hitPoints;
        public bool hasHealthPotion;
    }

    public State SwitchState()
    {
        if (!uniqueState)
            return SwitchStateInternal();

       return uniqueState.SwitchState();
    }

    public abstract State SwitchStateInternal();

    [Header(nameof(Time))]
    /// <summary>
    /// The time since the state was entered.
    /// </summary>
    public float TimeInState;

    /// <summary>
    /// The fixed time since the state was entered, for any physics tests.
    /// </summary>
    public float FixedTimeInState;

    public void EnterState()
    {
        TimeInState = 0;
        FixedTimeInState = 0;
        if (useStateVars)
            SetAgentVarsToStateVars();
        EnterInternal();
    }

    protected abstract void EnterInternal();

    public void UpdateState()
    {
        TimeInState += Time.deltaTime;
        UpdateInternal();
    }

    protected abstract void UpdateInternal();

    public void FixedUpdateState()
    {
        FixedTimeInState += Time.fixedDeltaTime;
        FixedUpdateInternal();
    }

    protected abstract void FixedUpdateInternal();

    public void ExitState()
    {
        creatureLogic.ResetAgentVars();
        TimeInState = 0;
        FixedTimeInState = 0;
        ExitInternal();
    }

    protected abstract void ExitInternal();

    public void SetAgentVarsToStateVars()
    {
        creatureLogic.SetAgentVars(stateAgentSpeed, stateAgentAcceleration, stateAgentStoppingDistance);
    }
}