using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCamera : MonoBehaviour
{
    Vector3 transformPosition;

    [SerializeField] private CinemachineVirtualCamera menuCam;
    [SerializeField] private CinemachineVirtualCamera creditCam;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject creditMenuUI;
    [SerializeField] private Button btn;

    [ButtonMethod]
    public void SwitchCamera()
    {
        creditCam.Priority = creditCam.Priority == 0 ? 1 : 0;
        StartCoroutine(ButtonCooldown());
        if (!creditMenuUI.activeSelf)
        {
            StartCoroutine(MenuSwitch1());
        }
        else
        {
            StartCoroutine(MenuSwitch2());
        }
    }

    IEnumerator MenuSwitch1()
    {
        mainMenuUI.SetActive(!mainMenuUI.activeSelf);
        yield return new WaitForSeconds(1f);
        creditMenuUI.SetActive(!creditMenuUI.activeSelf);
        yield break;
    }
    
    IEnumerator MenuSwitch2()
    {
        creditMenuUI.SetActive(!creditMenuUI.activeSelf);
        yield return new WaitForSeconds(1f);
        mainMenuUI.SetActive(!mainMenuUI.activeSelf);
        yield break;
    }

    IEnumerator ButtonCooldown()
    {
        btn.enabled = false;
        yield return new WaitForSeconds(1f);
        btn.enabled = true;
    }
}