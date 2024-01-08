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

    bool toggle;

    void Update()
    {
        Instance.imageInformationField.transform.position = Input.mousePosition + offset;
    }

    [ButtonMethod]
    public void ToggleSkillManager()
    {
        toggle = !toggle;
        skillTree.SetActive(toggle);

        if (skillTree != null && !skillTree.activeSelf)
            SetImageInformationField(false);
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
                oxygen += skillClass.skillPointAmount;
            }

            if (skillName == SkillClass.Skill.Pressure.ToString())
            {
                pressure += skillClass.skillPointAmount;
            }

            if (skillName == SkillClass.Skill.Temperature.ToString())
            {
                temperature += skillClass.skillPointAmount;
            }
        }
    }
}