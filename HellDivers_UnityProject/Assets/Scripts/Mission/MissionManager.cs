using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HELLDIVERS.UI.InGame;

public class MissionManager
{
    public static MissionManager Instance { get; private set; }
    public int MissionCount { get { return m_Missions[eMissionType.Tower].Count; } }

    private MissionFactory m_Factory = new MissionFactory();
    private Dictionary<eMissionType, List<Mission>> m_Missions = new Dictionary<eMissionType, List<Mission>>();

    public void Init()
    {
        if (Instance == null) Instance = this;
    }

    public void StartAllMission()
    {
        foreach (var missions in m_Missions)
        {
            foreach (Mission m in missions.Value)
            {
                m.StartMission();
            }
        }
    }

    public void CreateTowerMissions(uint num)
    {
        if (MapInfo.Instance == null) return;

        List<Transform> towerPositions = new List<Transform>();
        for (int i = 0; i < MapInfo.Instance.TowerPos.Count; i++)
        {
            towerPositions.Add(MapInfo.Instance.TowerPos[i]);
        }

        num = (num > towerPositions.Count) ? (uint)towerPositions.Count : num;

        for (int i = 0; i < num; i++)
        {
            int index = Random.Range(0, towerPositions.Count - 1);
            CreateMission(eMissionType.Tower, towerPositions[index]);
            towerPositions.RemoveAt(index);
        }
    }

    public void CreateMission(eMissionType type, Transform pos = null)
    {
        Mission mission = m_Factory.CreateMission(type);
        mission.OnMissionComplete += DoMissionFinished;

        if (pos != null)
        {
            mission.transform.parent = pos;
            mission.transform.position = pos.position;
        }

        List<Mission> pList;
        if (m_Missions.ContainsKey(type))
        {
            pList = m_Missions[type];
        }
        else
        {
            pList = new List<Mission>();
            m_Missions.Add(type, pList);
        }

        pList.Add(mission);
    }

    private void DoMissionFinished(Mission mission)
    {
        if (m_Missions.ContainsKey(mission.Type)) m_Missions[mission.Type].Remove(mission);
    }
}