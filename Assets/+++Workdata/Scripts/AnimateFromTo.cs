using MyBox;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AnimateFromTo : MonoBehaviour
{
    #region serialized fields
    [SerializeField] float animDelay = 1.5f;
    [SerializeField] float animTime = 3;
    [SerializeField] float animationStepsTimes = 0.9f;
    [SerializeField] AnimationCurve curve;
    [SerializeField] Transform reparentTarget;
    [SerializeField] Transform toTransform;
    [SerializeField] Transform target;
    [SerializeField] ParentSwapper swapper;
    [SerializeField] UnityEvent onFinished;
    #endregion

    #region private fields
    float animationSteps = 1;
    Vector2 originalPos;
    #endregion

    void Start()
    {
        animationSteps = animTime.RoundToInt() * 200;
        animationSteps *= animationStepsTimes;
        StartCoroutine(Animate());
    }
    IEnumerator Animate()
    {
        originalPos = target.position;
        yield return new WaitForSeconds(animDelay);
        swapper.Swap(reparentTarget, target);
        float progress = 0;

        for (int i = 0; i < animationSteps; i++)
        {
            target.position = Vector3.Lerp(originalPos, toTransform.position, Mathf.Clamp01(curve.Evaluate(progress)));
            progress += Time.deltaTime / animTime;
            yield return null;
        }
        swapper.UnSwap();
        onFinished?.Invoke();
        Destroy(gameObject);
    }
}