using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionPriority
{
    Main, Escape, Side
}

[System.Serializable]
public struct MissionReward
{
    public int EXP;
    public int Money;
}

public class Mission : MonoBehaviour
{
    public eMissionType Type { get { return m_Type; } }
    public MissionPriority Priority { get { return m_Priority; } }
    public bool IsFinished { get { return m_bFinished; } }
    public MissionReward Reward { get { return m_Reward; } }

    protected eMissionType m_Type;
    protected bool m_bFinished;
    protected MissionPriority m_Priority;
    protected MissionReward m_Reward;

    public delegate void MissionEventHolder();

    public delegate void MissionEventTracker(Mission mission);

    public MissionEventHolder OnMissionStart;
    public MissionEventHolder OnMissionComplete;

    public void StartMission()
    {
        OnStart();
        if (OnMissionStart != null) OnMissionStart();
    }

    protected virtual void OnStart()
    {
    }

    protected void CompleteMission()
    {
        if (OnMissionComplete != null) OnMissionComplete();
    }
}