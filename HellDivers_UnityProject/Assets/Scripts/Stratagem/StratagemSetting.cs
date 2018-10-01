using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StratagemSetting", menuName = "StratagemSetting", order = 100)]
public class StratagemSetting : ScriptableObject
{
    public string DisplayFolder = "Stratagems";
    public string ItemFolder = "Items";
    public GameObject DefaultDisplay;
    public GameObject DefaultItem;
}