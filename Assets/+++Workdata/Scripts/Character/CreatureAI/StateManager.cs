using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    #region serialized fields
    public State currentState;
    public State LastState => lastState;
    [SerializeField] State lastState;
    [SerializeField] List<State> lastStateIgnore = new();
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

        if (!newState) return;
        SetState(newState);
    }

    public void SetState(State newState)
    {
        if (currentState == newState) return;

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
}