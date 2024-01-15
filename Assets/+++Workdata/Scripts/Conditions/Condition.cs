using UnityEngine;

public class Condition : MonoBehaviour
{
    #region serialized fields
    [SerializeField, Range(0, 100)] protected float percentageDebuf = 0;
    [SerializeField] SkillClass.Skill skillType;
    public float AlphaDebuf => percentageDebuf / 100;
#pragma warning disable IDE0052
    [ShowOnly, SerializeField] float alphaDebuf;
#pragma warning restore IDE0052
    #endregion

    #region private fields
    public float Calc_percentageDebuff => -(-1 + Mathf.Clamp01(AlphaDebuf * -(-1 + SkillManager.Instance.GetSkillAmountAlpha(skillType))));
    #endregion

    public void SetPercentageDebuff(float _percentageDebuf)
    {
        percentageDebuf = _percentageDebuf;
    }
    public void SetPercentageDebuffAlpha(float _percentageAlpha)
    {
        _percentageAlpha = Mathf.Clamp01(_percentageAlpha);
        percentageDebuf = _percentageAlpha * 100;
        alphaDebuf = Calc_percentageDebuff;
    }
}