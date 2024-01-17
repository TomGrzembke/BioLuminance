using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InformationSO : ScriptableObject
{
    public string skillName;
    public List<SkillClass> skillList;
    [Tooltip("What this skill does TIP: Comma to start new line")]
    public string skillDescription;
    public int cost;

}

[Serializable]
public class SkillClass
{
    public Skill skill;
    public int skillPointAmount;

    public enum Skill
    {
        None,
        Oxygen,
        Pressure,
        Temperature
    }
}
