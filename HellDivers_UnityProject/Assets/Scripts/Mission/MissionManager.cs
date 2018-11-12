using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager
{
    public MissionManager Instance { get; private set; }
    public static int MissionCount { get; private set; }
    private static int m_TowerMissionNum;
    private List<Mission> m_MissionMap;

    public void Init()
    {
        if (Instance == null) Instance = this;

        m_MissionMap = new List<Mission>();
    }

    public void CreateTowerMissionsOnMap(uint num)
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
            CreateTowerMission(towerPositions[index]);
            towerPositions.RemoveAt(index);
        }
    }

    public void CreateTowerMission(Transform pos)
    {
        GameObject tower = new GameObject("MissionTower");
        tower.transform.parent = pos;
        tower.transform.position = pos.position;
        Mission mission = tower.AddComponent<MissionTower>();
        m_MissionMap.Add(mission);
        MissionCount++;
    }
}