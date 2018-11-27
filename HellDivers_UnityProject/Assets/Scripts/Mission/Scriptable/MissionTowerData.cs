using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionTowerSetting", menuName = "MissionTowerSetting", order = 1000)]
public class MissionTowerData : ScriptableObject
{
    public string ID { get { return m_Id; } }
    public float InteractRadius { get { return m_InteractRadius; } }
    public float ActivateRadius { get { return m_ActivateRadius; } }
    public float MinRadius { get { return m_MinRadius; } }
    public float MaxRadius { get { return m_MaxRadius; } }
    public float ActivatingTime { get { return m_ActivatingTime; } }
    public float MobSpawnTime { get { return m_MobSpawnTime; } }
    public int CodeLenghtMin { get { return m_CodeLenghtMin; } }
    public int CodeLenghtMax { get { return m_CodeLenghtMax; } }
    public int MobNum { get { return m_MobNum; } }

    [SerializeField] private string m_Id = "MissionTower";
    [SerializeField] private float m_InteractRadius = 4;
    [SerializeField] private float m_ActivateRadius = 15;
    [SerializeField] private float m_MinRadius = 20;
    [SerializeField] private float m_MaxRadius = 25;
    [SerializeField] private float m_ActivatingTime = 60;
    [SerializeField] private float m_MobSpawnTime = 10;
    [SerializeField] private int m_CodeLenghtMin = 6;
    [SerializeField] private int m_CodeLenghtMax = 8;
    [SerializeField] private int m_MobNum = 5;

    public void CopyTo(MissionTowerData other)
    {
        other.m_Id = this.m_Id;
        other.m_InteractRadius = this.m_InteractRadius;
        other.m_ActivateRadius = this.m_ActivateRadius;
        other.m_MinRadius = this.m_MinRadius;
        other.m_MaxRadius = this.m_MaxRadius;
        other.m_ActivatingTime = this.m_ActivatingTime;
        other.m_MobSpawnTime = this.m_MobSpawnTime;
        other.m_CodeLenghtMin = this.m_CodeLenghtMin;
        other.m_CodeLenghtMax = this.m_CodeLenghtMax;
        other.m_MobNum = this.m_MobNum;
    }

    public MissionTowerData Clone()
    {
        MissionTowerData newData = CreateInstance<MissionTowerData>();
        this.CopyTo(newData);
        return newData;
    }
}