using MyBox;
using System.Collections;
using UnityEngine;

public class AnimateFromTo : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Transform toTransform;
    [SerializeField] AnimationCurve curve;
    [SerializeField] Transform target;
    [SerializeField] ParentSwapper swapper;
    [SerializeField] Transform gfx;
    #endregion

    #region private fields
    int animationSteps = 1;
    [SerializeField] float animTime = 3;
    Vector2 originalPos;
    #endregion

    void Start()
    {
        animationSteps = animTime.RoundToInt() * 200;
        StartCoroutine(Animate());
    }
    IEnumerator Animate()
    {
        originalPos = target.position;
        yield return new WaitForSeconds(2);
        swapper.Swap(gfx, target);
        float progress = 0;

        for (int i = 0; i < animationSteps; i++)
        {
            target.position = Vector3.Lerp(originalPos, toTransform.position, Mathf.Clamp01(curve.Evaluate(progress)));
            progress += Time.deltaTime / animTime;
            yield return null;
        }
        swapper.UnSwap();
    }
}