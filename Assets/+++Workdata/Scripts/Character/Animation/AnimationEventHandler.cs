using UnityEngine;
using UnityEngine.Events;

/// <summary> This script should be attached to an animated Object to utilize the Unity Events through this </summary>
public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] AnimationUnityEvent[] onEvents;

    /// <param name="eventID"> The ID of the corresponding onEvent array ID</param>
    void OnAnimEventID(int eventID)
    {
        onEvents[eventID].animEvent?.Invoke();
    }

    /// <param name="eventID"> The ID of the corresponding onEvent array ID</param>
    void OnAnimEvent(AnimationEvent _animEvent)
    {
        int animeventID = DetermineAnimEventID(_animEvent);

        if (onEvents.Length <= animeventID) return;

        onEvents[animeventID].animEvent?.Invoke();

        if (onEvents[animeventID].eventNote == "")
            onEvents[animeventID].eventNote = _animEvent.stringParameter;
    }

    /// <summary> prefers the float parameter on an animEvent if its set, else takes the int parameter </summary>
    int DetermineAnimEventID(AnimationEvent _animEvent)
    {
        return _animEvent.intParameter != 0 ? _animEvent.intParameter : (int)_animEvent.floatParameter;
    }

    /// <summary> Be careful with statemachine blending and instantiating! Might need a spawn limit per frame bunch</summary>
    void InstantiateObj(Object obj)
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