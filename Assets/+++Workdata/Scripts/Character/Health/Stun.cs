using MyBox;
using System;
using System.Collections;
using UnityEngine;

public class Stun : MonoBehaviour
{
    public event Action<float> OnStunValueChanged;

    [SerializeField] float _maximumStun = 10;
    [SerializeField] float _currentStun;
    [SerializeField] float _stunFallofTime = 3f;
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

    [ButtonMethod]
    public void Test()
    {
        SetCurrentStun(_currentStun + _maximumStun * 20 / 100);
    }

    IEnumerator StunFallof()
    {
        yield return new WaitForSeconds(_stunFallofTime);

        for (int i = (int)_currentStun; i > 0; i--)
        {
            SetCurrentStun(_currentStun - 1, false);
            yield return new WaitForSeconds(_falloffTick);
        }
    }

    public void SetCurrentStun(float newStun, bool positiveValue = true)
    {
        if (newStun > _maximumStun)
            newStun = _maximumStun;

        if (_currentStun < 0)
            _currentStun = 0;

        _currentStun = newStun;

        if (positiveValue)
        {
            if (_stunFallofCo != null)
                StopCoroutine(_stunFallofCo);
            _stunFallofCo = StartCoroutine(StunFallof());
        }

        OnStunValueChanged?.Invoke(_currentStun);
    }


    public void RegisterForOnStunChanged(Action<float> callback, bool getInstantCallback = false)
    {
        OnStunValueChanged += callback;
        if (getInstantCallback)
            callback(_currentStun);
    }
}
