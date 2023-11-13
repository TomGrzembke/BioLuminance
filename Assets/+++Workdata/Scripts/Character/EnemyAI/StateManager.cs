using UnityEngine;

public class StateManager : MonoBehaviour
{
    #region serialized fields
    [SerializeField] State currentState;
    #endregion

    #region private fields

    AnimationManager animationManager;

    #endregion

    void FixedUpdate()
    {
        HandleStateMachine();
    }

    void HandleStateMachine()
    {
        if (currentState == null) return;

        State nextState = currentState.Tick(this, animationManager);
        SwitchToNextState(nextState);
    }

    void SwitchToNextState(State state)
    {
        currentState = state;
    }
}