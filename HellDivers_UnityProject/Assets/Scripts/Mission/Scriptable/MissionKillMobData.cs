using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionKillMobSetting", menuName = ("Mission Kill Mob Setting"), order = 1000)]
public class MissionKillMobData : MissionData
{
    public int KillAmount { get { return m_KillAmout; } }

    [SerializeField] private int m_KillAmout;
}