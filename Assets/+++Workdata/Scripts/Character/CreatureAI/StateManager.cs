using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    #region serialized fields
    public State currentState;
    public State LastState => lastState;
    [SerializeField] State lastState;
    [SerializeField] List<State> lastStateIgnore = new();

    [Header("States")]
    [SerializeField] StatusManager statusManager;
    [SerializeField] State stunState;
    [SerializeField] State deathState;
    [SerializeField] State fleeState;
    [Range(0, 1), SerializeField] float fleeHealthPercentage = .2f;

    [Header("Health")]
    [SerializeField] LimbManager limbManager;
    #endregion

    #region private fields
    #endregion

    void Start()
    {
        if (currentState)
            currentState.EnterState();
    }

    void FixedUpdate()
    {
        if (currentState != null)
            currentState.FixedUpdateState();
    }

    void Update()
    {
        HandleStateMachine();
        if (currentState != null)
            currentState.UpdateState();
    }

    void HandleStateMachine()
    {
        if (!currentState) return;
        State newState = currentState.SwitchState();

        SetState(newState);
    }

    public void SetState(State newState)
    {
        if (!currentState) return;
        if (!newState) return;
        if (currentState == newState) return;
        if (currentState == deathState) return;

        currentState.ExitState();
        SetLastState(currentState);
        currentState = newState;
        currentState.EnterState();
    }

    void SetLastState(State _state)
    {
        if (!lastStateIgnore.Contains(_state))
            lastState = currentState;
    }

    void OnEnable()
    {
        statusManager.StunSunject.RegisterOnStun(OnStun);
        statusManager.HealthSubject.RegisterOnHealthChangedAlpha(OnHealthChangedAlpha);
    }

    void OnDisable()
    {
        statusManager.StunSunject.OnStun -= OnStun;
        statusManager.HealthSubject.OnHealthChangedAlpha -= OnHealthChangedAlpha;
    }

    void OnStun(bool condition)
    {
        if(currentState == deathState) return;

        if (condition)
        {
            SetState(stunState);
            return;
        }

        SetState(LastState);
    }

    void OnHealthChangedAlpha(float alpha)
    {
        if (currentState == deathState) return;

        if (alpha <= 0)
        {
            SetState(deathState);
            return;
        }

        if (alpha <= fleeHealthPercentage)
        {
            SetState(fleeState);
            return;
        }

        if (currentState == fleeState)
            SetState(LastState);
    }
}