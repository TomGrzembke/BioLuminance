using System;
using UnityEngine;
using MyBox;

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

    [SerializeField] StateEvent[] stateEvent;

    #endregion

    void Awake() => creatureLogic = GetComponentInParent<CreatureLogic>();

    #region StateEvent
    public enum SwitchReason
    {
        none,
        time,
        distanceTraveled,
        damage,
    }
    public enum DamageType
    {
        none,
        amount,
        hit
    }

    [Serializable]
    public struct StateEvent
    {
        public State switchState;
        [ConditionalField(nameof(switchState), false)] 
        public SwitchReason reason;

        //time
        [ConditionalField(nameof(reason), false, SwitchReason.time)]
        public float time;

        //distanceTraveled
        [ConditionalField(nameof(reason), false, SwitchReason.distanceTraveled)]
        public float distance;

        //distanceTraveled
        [ConditionalField(nameof(reason), false, SwitchReason.damage)]
        public DamageType damageType;
        [ConditionalField(nameof(damageType), false, DamageType.amount)]
        public float damage;
        [ConditionalField(nameof(damageType), false, DamageType.hit)]
        public float hitAmount;
    }
    #endregion

    public State CheckStateEvent()
    {
        for (int i = 0; i < stateEvent.Length; i++)
        {
            switch (stateEvent[i].reason)
            {
                case SwitchReason.time:

                    break;
                case SwitchReason.distanceTraveled:
                    break;
                case SwitchReason.damage:
                    break;
                default:
                    break;
            }
        }

        return false;
    }

    public State SwitchState()
    {
        if (!uniqueState && !stateEvent[0].switchState)
            return CheckStateEvent();
        else if(!uniqueState)
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