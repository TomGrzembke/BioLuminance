using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour
{
    #region serialized fields
    [SerializeField] UnityEvent onTriggerEnterEvent;
    [SerializeField] UnityEvent onTriggerExitEvent;
    [SerializeField] string tagToFilter = "player";
    #endregion

    #region private fields

    #endregion
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagToFilter))
        {
            onTriggerEnterEvent?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(tagToFilter))
        {
            onTriggerExitEvent?.Invoke();
        }
    }
}