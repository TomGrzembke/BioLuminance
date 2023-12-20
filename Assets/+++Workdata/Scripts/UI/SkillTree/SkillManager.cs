using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class SkillManager : MonoBehaviour
{
    public Vector3 offset;
    public GameObject imageInformationField;
    
    void Update()
    {
        imageInformationField.transform.position = Input.mousePosition + offset;
    }
}