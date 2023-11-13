using System;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    #region serialized fields
    [SerializeField] State currentState;
    [SerializeField] State lastState;
    #endregion

    #region private fields

    AnimationManager animationManager;

    #endregion

    void FixedUpdate()
    {
        if(currentState != null)
            currentState.FixedUpdateState();
    }

    void Update()
    {
        HandleStateMachine();
        if(currentState != null)
            currentState.UpdateState();
    }

    void Awake()
    {
        
    }

    void HandleStateMachine()
    {
        if (currentState == null) return;
        
        State newState = currentState.SwitchState();
        if (currentState != newState)
        {
            lastState = currentState;
            lastState.ExitState();
            currentState = newState;
            currentState.EnterState();
        }
    }
}