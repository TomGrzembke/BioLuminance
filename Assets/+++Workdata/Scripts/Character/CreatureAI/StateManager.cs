using UnityEngine;

public class StateManager : MonoBehaviour
{
    #region serialized fields
    [SerializeField] State currentState;
    public State LastState => lastState;
    [SerializeField] State lastState;
    #endregion

    #region private fields
    #endregion

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
        if (currentState == null) return;

        State newState = currentState.SwitchStateInternal();
        if (currentState != newState)
        {
            lastState = currentState;
            lastState.ExitState();
            currentState = newState;
            currentState.EnterState();
        }
    }

    public void SetState(State _state)
    {
        lastState = currentState;
        currentState = _state;
    }
}