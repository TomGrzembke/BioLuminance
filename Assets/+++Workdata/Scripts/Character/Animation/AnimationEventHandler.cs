using MyBox;
using UnityEngine;
using UnityEngine.Events;

/// <summary> This script should be attached to an animated Object to utilize the Unity Events through this </summary>
public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] UnityEvent[] onEvent;
    [SerializeField] AnimationUnityEvent[] onEvents;

    /// <param name="eventID"> The ID of the corresponding onEvent array ID</param>
    public void OnEvent(int eventID)
    {
        onEvent[eventID].Invoke();
    }

    /// <param name="eventID"> The ID of the corresponding onEvent array ID</param>
    public void OnEvents(AnimationEvent _animEvent)
    {
        if (onEvents.Length <= _animEvent.intParameter) return;

        onEvents[_animEvent.intParameter].animEvent?.Invoke();

        if (onEvents[_animEvent.intParameter].eventNote == "")
            onEvents[_animEvent.intParameter].eventNote = _animEvent.stringParameter;
    }

    /// <summary> Be careful with statemachine blending and instantiating! Might need a spawn limit per frame bunch</summary>
    public void InstantiateObj(Object obj)
    {
        Instantiate(obj, transform.localPosition, Quaternion.identity);
    }
}

[System.Serializable]
public struct AnimationUnityEvent
{
    [Tooltip("To keep track of the animation functionality")]
    public string eventNote;
    public UnityEvent animEvent;
}