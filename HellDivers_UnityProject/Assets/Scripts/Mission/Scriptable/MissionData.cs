using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionData : ScriptableObject
{
    public eMissionType Type { get { return m_Type; } }
    public MissionReward Reward { get { return m_Reward; } }

    [SerializeField] private eMissionType m_Type;
    [SerializeField] private MissionReward m_Reward;
}