using MyBox;
using System;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    #region serialized fields
    [Header(nameof(Time))]
    /// <summary>
    /// The time since the state was entered.
    /// </summary>
    [ConditionalField(nameof(uniqueState), true)] public float TimeInState;

    [ConditionalField(nameof(uniqueState), true), SerializeField] float distanceTravelled;
    public float DistanceTravelled => distanceTravelled;
    Vector3 oldPos;

    /// <summary>
    /// The fixed time since the state was entered, for any physics tests.
    /// </summary>
    [ConditionalField(nameof(uniqueState), true)] public float FixedTimeInState;
    [Header(nameof(State))]
    [SerializeField] protected State uniqueState;

    protected CreatureLogic creatureLogic;
    public float StateAgentSpeed => stateAgentSpeed;
    [ConditionalField(nameof(uniqueState), true), SerializeField] protected float stateAgentSpeed = 3.5f;

    public float StateAgentAcceleration => stateAgentAcceleration;
    [ConditionalField(nameof(uniqueState), true), SerializeField] protected float stateAgentAcceleration = 5f;

    public float StateAgentStoppingDistance => stateAgentStoppingDistance;
    [ConditionalField(nameof(uniqueState), true), SerializeField] protected float stateAgentStoppingDistance = 1f;
    [ConditionalField(nameof(uniqueState), true), SerializeField] bool useStateVars;
    public bool Dangerous => dangerous;
    [ConditionalField(nameof(uniqueState), true), SerializeField] bool dangerous;

    [SerializeField] StateEvent[] stateEvent;

    #endregion

    void Awake()
    {
        creatureLogic = GetComponentInParent<CreatureLogic>();

        if (creatureLogic == null)
            transform.parent.GetComponentInParent<CreatureLogic>();
    }


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
                    if (stateEvent[i].time < TimeInState)
                        return stateEvent[i].switchState;
                    break;
                case SwitchReason.distanceTraveled:
                    if (stateEvent[i].distance < distanceTravelled)
                        return stateEvent[i].switchState;
                    break;
                case SwitchReason.damage:
                    break;
                default:
                    break;
            }
        }

        return SwitchStateInternal();
    }

    public State SwitchState()
    {
        if (!uniqueState)
            if (stateEvent.Length > 0)
                return CheckStateEvent();
            else
                return SwitchStateInternal();

        return uniqueState.SwitchState();
    }

    public abstract State SwitchStateInternal();


    public void EnterState()
    {
        TimeInState = 0;
        FixedTimeInState = 0;
        oldPos = transform.position;
        distanceTravelled = 0;
        if (useStateVars)
            SetAgentVarsToStateVars();
        EnterInternal();
    }

    protected abstract void EnterInternal();

    public void UpdateState()
    {
        TimeInState += Time.deltaTime;
        distanceTravelled += Vector3.Distance(transform.position, oldPos);
        oldPos = transform.position;
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
        distanceTravelled = 0;
        ExitInternal();
    }

    protected abstract void ExitInternal();

    public void SetAgentVarsToStateVars()
    {
        creatureLogic.SetAgentVars(stateAgentSpeed, stateAgentAcceleration, stateAgentStoppingDistance);
    }
}