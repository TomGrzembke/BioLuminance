using MyBox;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
    void Awake() => Instance = this;
    
    [SerializeField] GameObject imageInformationField;
    [SerializeField] GameObject skillTree;

    public int Pressure => pressure;
    [SerializeField] int pressure;
    public int Temperature => temperature;
    [SerializeField] int temperature;
    public int Oxygen => oxygen;
    [SerializeField] int oxygen;

    [SerializeField] float maxPointAmount = 4;
    void Update()
    {
        imageInformationField.transform.position = Input.mousePosition;
    }

    [ButtonMethod]
    public static void OpenSkillManager()
    {
        Instance.skillTree.SetActive(true);

        if (Instance.skillTree != null && !Instance.skillTree.activeSelf)
            Instance.SetImageInformationField(false);
    }

    [ButtonMethod]
    public static void CloseSkillManager()
    {
        Instance.skillTree.SetActive(false);

        if (Instance.skillTree != null && !Instance.skillTree.activeSelf)
            Instance.SetImageInformationField(false);
    }

    public void SetImageInformationField(bool condition)
    {
        Instance.imageInformationField.SetActive(condition);
    }

    public void SkillUpdate(InformationSO informationSO)
    {
        foreach (SkillClass skillClass in informationSO.skillList)
        {
            string skillName = skillClass.skill.ToString();

            if (skillName == SkillClass.Skill.None.ToString())
            {
                print("Assign Skill in SkillList");
                return;
            }

            if (skillName == SkillClass.Skill.Oxygen.ToString())
            {
                Instance.oxygen += skillClass.skillPointAmount;
            }

            if (skillName == SkillClass.Skill.Pressure.ToString())
            {
                Instance.pressure += skillClass.skillPointAmount;
            }

            if (skillName == SkillClass.Skill.Temperature.ToString())
            {
                Instance.temperature += skillClass.skillPointAmount;
            }
        }
    }

    public int GetSkillAmount(SkillClass.Skill skillType)
    {
        return skillType switch
        {
            SkillClass.Skill.Oxygen => Instance.oxygen,
            SkillClass.Skill.Pressure => Instance.pressure,
            SkillClass.Skill.Temperature => Instance.temperature,
            _ => 0,
        };
    }

    public float GetSkillAmountAlpha(SkillClass.Skill skillType)
    {
        return skillType switch
        {
            SkillClass.Skill.Oxygen => oxygen / maxPointAmount,
            SkillClass.Skill.Pressure => pressure / maxPointAmount,
            SkillClass.Skill.Temperature => temperature / maxPointAmount,
            _ => 0,
        };
    }
}