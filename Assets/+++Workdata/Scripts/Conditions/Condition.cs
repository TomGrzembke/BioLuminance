using UnityEngine;

public class Condition : MonoBehaviour
{
    #region serialized fields
    [SerializeField, Range(0, 100)] protected float percentageEffectiveness = 0;
    [SerializeField] SkillClass.Skill skillType;
    #endregion

    #region private fields
    protected float calc_percentageEffectiveness => percentageEffectiveness * SkillManager.Instance.GetSkillAmountAlpha(skillType);
    #endregion

    public void SetPercentageEffectiveness(float _percentageEffectiveness)
    {
        percentageEffectiveness = _percentageEffectiveness;
    }
}