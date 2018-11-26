using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BGMSetting", menuName = "BGM Setting", order = 100)]
public class BGMSetting : ScriptableObject
{
    public AudioClip Theme;
    public List<AudioClip> BattleField;
    public AudioClip MissionSuccess;
    public AudioClip MissionFailed;
}