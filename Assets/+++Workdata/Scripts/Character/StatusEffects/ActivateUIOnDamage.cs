using System.Collections;
using UnityEngine;

public class ActivateUIOnDamage : MonoBehaviour
{
    #region serialized fields
    [SerializeField] HealthSubject healthSubject;
    [SerializeField] CanvasGroup uiCanvasGroup;
    [SerializeField] float timeTillFullVisible = 1;
    #endregion

    #region private fields
    Coroutine activateRoutine;
    #endregion

    void OnEnable()
    {
        healthSubject.RegisterOnHealthChanged(ActivateUI);
    }

    void OnDisable()
    {
        healthSubject.OnHealthChanged -= ActivateUI;
    }

    void ActivateUI(float _)
    {
        if (activateRoutine == null)
            activateRoutine = StartCoroutine(ActivateUICor());
    }

    IEnumerator ActivateUICor()
    {
        float timeWentBy = 0;
        while (timeWentBy < timeTillFullVisible)
        {
            timeWentBy += Time.deltaTime;
            uiCanvasGroup.alpha = timeWentBy / timeTillFullVisible;
            yield return null;
        }
        this.enabled = false;
    }
}