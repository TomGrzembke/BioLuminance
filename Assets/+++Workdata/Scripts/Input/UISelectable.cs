using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UISelectable : MonoBehaviour
{
    Button selectable;
    //PlayerInputActions inputActions;

    void Awake()
    {
        //inputActions = new();
        selectable = GetComponent<Button>();
    }
    void Start()
    {
        Select();
    }

    void Select()
    {
        StartCoroutine(DelayedSelection(selectable));
    }

    IEnumerator DelayedSelection(Selectable selectable)
    {
        yield return null;
        selectable.Select();
    }

    void OnEnable()
    {
        StartCoroutine(DelayedSelection(selectable));
    }
    void OnDisable()
    {
        StopCoroutine(DelayedSelection(selectable));
    }
}
