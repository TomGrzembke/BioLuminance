using UnityEngine;

public class SlowCondition : MonoBehaviour
{
    #region serialized fields
    [SerializeField, Range(0, 100)] float percentageEffectiveness = 100;
    public float AlphaEffectiveness => percentageEffectiveness / 100;
    #endregion

    #region private fields

    #endregion

    public void SetPercentageEffectiveness(float _percentageEffectiveness)
    {
        _percentageEffectiveness = Mathf.Clamp(_percentageEffectiveness, 0, 100);
        percentageEffectiveness = _percentageEffectiveness;
    }
}