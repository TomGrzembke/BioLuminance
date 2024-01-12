using UnityEngine;

public class Condition : MonoBehaviour
{
    #region serialized fields
    [SerializeField, Range(0, 100)] protected float percentageDebuf = 0;
    [SerializeField] SkillClass.Skill skillType;
    public float AlphaDebuf => percentageDebuf / 100;
    #endregion

    #region private fields
    public float Calc_percentageDebuff => -(-1 + Mathf.Clamp01(AlphaDebuf * -(-1 + SkillManager.Instance.GetSkillAmountAlpha(skillType))));
    #endregion

    public void SetPercentageEffectiveness(float _percentageEffectiveness)
    {
        percentageDebuf = _percentageEffectiveness;
    }
}