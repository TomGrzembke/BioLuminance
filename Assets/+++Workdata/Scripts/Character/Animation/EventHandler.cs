
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary> This script should be attached to an animated Object to utilize the Unity Events through this </summary>
public class EventHandler : MonoBehaviour
{
    [SerializeField] UnityEvent onEvent1;
    [SerializeField] UnityEvent onEvent2;
    [SerializeField] UnityEvent[] onEvent;

    /// <summary> This can be called from animation events or from other scripts </summary>
    public void OnEvent1()
    {
        onEvent1.Invoke();
    }
    public void OnEvent2()
    {
        onEvent2.Invoke();
    }
    public void OnEvent(int eventID)
    {
        onEvent2.Invoke();
    }
}

