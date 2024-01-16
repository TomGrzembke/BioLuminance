using MyBox;
using System;
using System.Collections;
using UnityEngine;

public class StunSubject : MonoBehaviour
{
    enum StunState
    {
        unstunned,
        stunDescending,
        stunned
    }

    [SerializeField] StunState stunState;

    public event Action<float> OnStunValueChanged;
    public event Action<float> OnStunValueChangedAlpha;
    public event Action<bool> OnStun;

    [SerializeField] float maximumStun = 10;
    [SerializeField] float currentStun;
    [SerializeField] float stunFallofTime = 3f;
    [SerializeField] float stunTime = 2f;
    [SerializeField] float falloffTick = 0.2f;

    Coroutine stunFallofCo;

    public float CurrentStun
    {
        get => currentStun;
        set => StunLogic(value);
    }

    public void AddStun(float additionalStun)
    {
        StunLogic(currentStun + additionalStun);
    }

    public void AddStunPercentage(float percent)
    {
        StunLogic(currentStun + maximumStun * percent / 100);
    }

    [ButtonMethod]
    public void AddStun20Percentage()
    {
        StunLogic(currentStun + maximumStun * 20 / 100);
    }


    IEnumerator StunFallof()
    {
        yield return new WaitForSeconds(stunFallofTime);

        for (int i = (int)Mathf.Ceil(currentStun); i > 0; i--)
        {
            stunState = StunState.stunDescending;

            StunLogic(currentStun - 1, false);
            yield return new WaitForSeconds(falloffTick);
        }
        stunState = StunState.unstunned;
    }

    IEnumerator Stunned()
    {
        if (stunFallofCo != null)
            StopCoroutine(stunFallofCo);

        stunState = StunState.stunned;
        OnStun?.Invoke(true);

        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(stunTime / 5);

            SetStun(currentStun - maximumStun / 5);
        }

        stunState = StunState.unstunned;
        OnStun?.Invoke(false);
    }

    public void StunLogic(float newStun, bool positiveValue = true)
    {
        if (stunState == StunState.stunned) return;

        SetStun(newStun);

        if (currentStun == maximumStun)
        {
            StartCoroutine(Stunned());
            return;
        }

        if (positiveValue)
        {
            if (stunFallofCo != null)
                StopCoroutine(stunFallofCo);
            stunFallofCo = StartCoroutine(StunFallof());
        }
    }

    private float SetStun(float newStun)
    {
        newStun = Mathf.Clamp(newStun, 0, maximumStun);

        currentStun = newStun;

        OnStunValueChanged?.Invoke(currentStun);
        OnStunValueChangedAlpha?.Invoke(currentStun / maximumStun);
        return newStun;
    }

    public void RegisterOnStunChanged(Action<float> callback, bool getInstantCallback = false)
    {
        OnStunValueChanged += callback;
        if (getInstantCallback)
            callback(currentStun);
    }

    public void RegisterOnStunChangedAlpha(Action<float> callback, bool getInstantCallback = false)
    {
        OnStunValueChangedAlpha += callback;
        if (getInstantCallback)
            callback(currentStun / maximumStun);
    }

    public void RegisterOnStun(Action<bool> callback)
    {
        OnStun += callback;
    }
}
