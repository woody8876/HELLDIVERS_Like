using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StratagemSetting", menuName = "StratagemSetting", order = 100)]
public class StratagemSetting : ScriptableObject
{
    public string DisplayFolder = "Stratagems";
    public string ResultFolder = "StratagemSupplys";
    public float MaxScaleThrowForce;
    public float ScaleThorwForceRate;
    public GameObject DefaultDisplay;
    public GameObject DefaultResult;
}