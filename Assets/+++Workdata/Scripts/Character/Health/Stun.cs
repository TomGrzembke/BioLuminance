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

    [SerializeField] float _maximumStun = 10;
    [SerializeField] float _currentStun;
    [SerializeField] float _stunFallofTime = 3f;
    [SerializeField] float _stunTime = 2f;
    [SerializeField] float _falloffTick = 0.2f;

    Coroutine _stunFallofCo;

    public float CurrentStun
    {
        get => _currentStun;
        set => SetCurrentStun(value);
    }

    public void AddStun(float additionalHealth)
    {
        SetCurrentStun(_currentStun + additionalHealth);
    }

    public void AddStunPercentage(float percent)
    {
        SetCurrentStun(_currentStun + _maximumStun * percent / 100);
    }

    IEnumerator StunFallof()
    {
        yield return new WaitForSeconds(_stunFallofTime);

        for (int i = (int)_currentStun; i > 0; i--)
        {
            if (i != 0)
                stunState = StunState.stunDescending;

            SetCurrentStun(_currentStun - 1, false);
            yield return new WaitForSeconds(_falloffTick);
        }
        stunState = StunState.unstunned;
    }

    IEnumerator Stunned()
    {
        if (_stunFallofCo != null)
            StopCoroutine(_stunFallofCo);

        stunState = StunState.stunned;

        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(_stunTime / 5);
            _currentStun -= _maximumStun / 5;
        }

        stunState = StunState.unstunned;
    }

    public void SetCurrentStun(float newStun, bool positiveValue = true)
    {
        if (stunState == StunState.stunned) return;
        OnStunValueChanged?.Invoke(_currentStun);

        if (newStun >= _maximumStun)
            newStun = _maximumStun;
        else if (newStun <= 0)
            newStun = 0;

        _currentStun = newStun;

        if (_currentStun == _maximumStun)
        {
            StartCoroutine(Stunned());
            return;
        }

        if (positiveValue)
        {
            if (_stunFallofCo != null)
                StopCoroutine(_stunFallofCo);
            _stunFallofCo = StartCoroutine(StunFallof());
        }
    }


    public void RegisterForOnStunChanged(Action<float> callback, bool getInstantCallback = false)
    {
        OnStunValueChanged += callback;
        if (getInstantCallback)
            callback(_currentStun);
    }
}
