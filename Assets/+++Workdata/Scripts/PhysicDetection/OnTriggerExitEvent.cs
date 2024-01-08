using UnityEngine;
using UnityEngine.Events;

public class OnTriggerExitEvent : MonoBehaviour
{
    #region serialized fields
    [SerializeField] UnityEvent onTriggerExitEvent;
    [SerializeField] string tagToFilter;
    #endregion

    #region private fields

    #endregion

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(tagToFilter))
        {
            onTriggerExitEvent?.Invoke();
        }
    }
}