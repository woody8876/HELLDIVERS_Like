using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionTowerSetting", menuName = "MissionTowerSetting", order = 1000)]
public class MissionTowerData : MissionData
{
    public string ID { get { return m_Id; } }
    public float InteractRadius { get { return m_InteractRadius; } }
    public float ActivateRadius { get { return m_ActivateRadius; } }
    public float MinRadius { get { return m_MinRadius; } }
    public float MaxRadius { get { return m_MaxRadius; } }
    public float ActivatingTime { get { return m_ActivatingTime; } }
    public int CodeLenghtMin { get { return m_CodeLenghtMin; } }
    public int CodeLenghtMax { get { return m_CodeLenghtMax; } }
    public List<PhaseData> PhaseDatas { get { return m_PhaseDatas; } }

    [SerializeField] private string m_Id = "MissionTower";
    [SerializeField] private float m_InteractRadius = 4;
    [SerializeField] private float m_ActivateRadius = 15;
    [SerializeField] private float m_MinRadius = 20;
    [SerializeField] private float m_MaxRadius = 25;
    [SerializeField] private float m_ActivatingTime = 60;
    [SerializeField] private int m_CodeLenghtMin = 6;
    [SerializeField] private int m_CodeLenghtMax = 8;
    [SerializeField] private List<PhaseData> m_PhaseDatas;

    public void CopyTo(MissionTowerData other)
    {
        other.m_Id = this.m_Id;
        other.m_InteractRadius = this.m_InteractRadius;
        other.m_ActivateRadius = this.m_ActivateRadius;
        other.m_MinRadius = this.m_MinRadius;
        other.m_MaxRadius = this.m_MaxRadius;
        other.m_ActivatingTime = this.m_ActivatingTime;
        other.m_CodeLenghtMin = this.m_CodeLenghtMin;
        other.m_CodeLenghtMax = this.m_CodeLenghtMax;
        other.m_PhaseDatas = this.m_PhaseDatas;
    }

    public MissionTowerData Clone()
    {
        MissionTowerData newData = CreateInstance<MissionTowerData>();
        this.CopyTo(newData);
        return newData;
    }
}

[System.Serializable]
public class PhaseData : IComparable<PhaseData>
{
    public float Time { get { return m_Time; } }
    public int FishNum { get { return m_FishNum; } }
    public int FishVariantNum { get { return m_FishVariantNum; } }
    public int PatrolNum { get { return m_PatrolNum; } }
    public int TankNum { get { return m_TankNum; } }

    [SerializeField] private float m_Time;
    [SerializeField] private int m_FishNum;
    [SerializeField] private int m_FishVariantNum;
    [SerializeField] private int m_PatrolNum;
    [SerializeField] private int m_TankNum;

    public int CompareTo(PhaseData other)
    {
        if (this.Time > other.Time) return 1;
        else if (this.Time < other.Time) return -1;
        else return 0;
    }
}