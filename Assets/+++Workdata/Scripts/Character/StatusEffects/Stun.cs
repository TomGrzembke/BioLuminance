using MyBox;
using System;
using System.Collections;
using UnityEngine;

public class Stun : MonoBehaviour
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

    [SerializeField] float maximumStun = 10;
    [SerializeField] float currentStun;
    [SerializeField] float stunFallofTime = 3f;
    [SerializeField] float stunTime = 2f;
    [SerializeField] float falloffTick = 0.2f;

    Coroutine stunFallofCo;

    public float CurrentStun
    {
        get => currentStun;
        set => SetCurrentStun(value);
    }

    public void AddStun(float additionalHealth)
    {
        SetCurrentStun(currentStun + additionalHealth);
    }

    public void AddStunPercentage(float percent)
    {
        SetCurrentStun(currentStun + maximumStun * percent / 100);
    }

    [ButtonMethod]
    public void AddStun20Percentage()
    {
        SetCurrentStun(currentStun + maximumStun * 20 / 100);
    }


    IEnumerator StunFallof()
    {
        yield return new WaitForSeconds(stunFallofTime);

        for (int i = (int)currentStun; i > 0; i--)
        {
            if (i != 0)
                stunState = StunState.stunDescending;

            SetCurrentStun(currentStun - 1, false);
            yield return new WaitForSeconds(falloffTick);
        }
        stunState = StunState.unstunned;
    }

    IEnumerator Stunned()
    {
        if (stunFallofCo != null)
            StopCoroutine(stunFallofCo);

        stunState = StunState.stunned;

        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(stunTime / 5);
            currentStun -= maximumStun / 5;
        }

        stunState = StunState.unstunned;
    }

    public void SetCurrentStun(float newStun, bool positiveValue = true)
    {
        if (stunState == StunState.stunned) return;
        OnStunValueChanged?.Invoke(currentStun);
        OnStunValueChangedAlpha?.Invoke(currentStun / maximumStun);

        if (newStun >= maximumStun)
            newStun = maximumStun;
        else if (newStun <= 0)
            newStun = 0;

        currentStun = newStun;

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
}
