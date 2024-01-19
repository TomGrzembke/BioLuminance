using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MyBox;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    Vector3 transformPosition;

    [SerializeField] private CinemachineVirtualCamera menuCam;
    [SerializeField] private CinemachineVirtualCamera creditCam;

    [ButtonMethod]
    public void SwitchCamera()
    {
        creditCam.Priority = (creditCam.Priority == 0) ? 1 : 0;
    }
}