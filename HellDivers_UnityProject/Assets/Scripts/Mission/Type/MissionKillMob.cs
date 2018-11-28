using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionKillMob : Mission
{
    public int CurrentAmount { get { return m_CurrentAmount; } }

    private MissionKillMobData m_Data;
    private int m_CurrentAmount;

    public void Initialize(MissionKillMobData data)
    {
        m_Data = data;
        m_Priority = data.Priority;
        m_Type = data.Type;
        m_Reward = data.Reward;
    }

    protected override void OnStart()
    {
        m_CurrentAmount = 0;
        m_bFinished = false;

        if (MobManager.m_Instance != null)
        {
            MobManager.m_Instance.OnKill += CountMobBeKill;
        }
    }

    private void CountMobBeKill()
    {
        m_CurrentAmount++;

        if (m_CurrentAmount >= m_Data.KillAmount)
        {
            m_bFinished = true;
            CompleteMission();
        }
    }
}