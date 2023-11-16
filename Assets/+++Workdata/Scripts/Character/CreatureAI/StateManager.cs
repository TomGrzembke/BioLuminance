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

        if (currentState == newState) return;
        if (!newState) return;

        lastState = currentState;
        lastState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public void SetState(State _state)
    {
        lastState = currentState;
        currentState = _state;
    }
}