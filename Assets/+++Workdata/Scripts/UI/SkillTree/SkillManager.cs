using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class SkillManager : MonoBehaviour
{
    public Vector3 offset;
    public GameObject imageInformationField;
    public GameObject skillTree;

    //TODO Transfer skill to other script
    
    public float pressure;
    public float temperature;
    public float oxygen;

    PlayerInputActions playerControls;
    bool toggle;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerControls.UserInterface.SkillTree.performed += ctx => pressedEsc();

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        imageInformationField.transform.position = Input.mousePosition + offset;
    }

    public void pressedEsc()
    {
        if (playerControls.UserInterface.SkillTree.triggered)
        {
            toggle = !toggle;
            skillTree.SetActive(toggle);
        }

        if (skillTree != null && !skillTree.activeSelf)
            imageInformationField.SetActive(false);
    }

    public void SkillUpdate(InformationSO informationSO)
    {
        foreach (SkillClass skillClass in informationSO.skillList)
        {
            string skillName = skillClass.skill.ToString();

            if (skillName == "None")
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