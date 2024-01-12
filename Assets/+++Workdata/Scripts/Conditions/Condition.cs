using UnityEngine;

public class Condition : MonoBehaviour
{
    #region serialized fields
    [SerializeField, Range(0, 100)] protected float percentageEffectiveness = 100;
    #endregion

    #region private fields

    #endregion

    public void SetPercentageEffectiveness(float _percentageEffectiveness)
    {
        percentageEffectiveness = _percentageEffectiveness;
    }
}