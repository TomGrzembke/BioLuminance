using UnityEngine;

public class SkillpointInitializer : MonoBehaviour
{
    #region serialized fields
    [SerializeField] SkillNode[] temperatureNodes;
    [SerializeField] SkillNode[] oxygenNodes;
    [SerializeField] SkillNode[] pressureNodes;
    [SerializeField] SkillManager skillManager;
    #endregion

    #region private fields

    #endregion

    void Start()
    {
        for (int i = 0; i < skillManager.Temperature; i++)
        {
            temperatureNodes[i].enabled = true;
            temperatureNodes[i].Initialize();
        }

        for (int i = 0; i < skillManager.Oxygen; i++)
        {
            oxygenNodes[i].enabled = true;
            oxygenNodes[i].Initialize();
        }

        for (int i = 0; i < skillManager.Pressure; i++)
        {
            pressureNodes[i].enabled = true;
            pressureNodes[i].Initialize();
        }
    }
}