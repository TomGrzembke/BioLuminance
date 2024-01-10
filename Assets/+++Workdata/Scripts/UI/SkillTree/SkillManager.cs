using MyBox;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    static SkillManager Instance;
    void Awake() => Instance = this;

    [SerializeField] Vector3 offset;
    [SerializeField] GameObject imageInformationField;
    [SerializeField] GameObject skillTree;

    //TODO Transfer skill to other script

    [SerializeField] float pressure;
    [SerializeField] float temperature;
    [SerializeField] float oxygen;

    void Update()
    {
        Instance.imageInformationField.transform.position = Input.mousePosition + offset;
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
}