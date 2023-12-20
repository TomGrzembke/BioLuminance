using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InformationSO : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public Color acquired = new Color32(0, 255, 0, 255);
    public Color notAcquired = new Color32(255, 0, 0, 255);
}