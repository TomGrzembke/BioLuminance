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
    [Space(15)]
    public Color acquired = new Color32(0, 255, 0, 255);
    public Color notAcquired = new Color32(255, 0, 0, 255);
}

[Serializable]
public class SkillClass
{
    public Skill skill;
    public float skillPointAmount;

    public enum Skill
    {
        None,
        Oxygen,
        Pressure,
        Temperature
    }
}
