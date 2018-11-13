using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HELLDIVERS.UI.InGame;

public class MissionManager
{
    public MissionManager Instance { get; private set; }
    public int MissionCount { get { return m_MissionMap.Count; } }
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
        MissionTower mission = tower.AddComponent<MissionTower>();
        m_MissionMap.Add(mission);
        mission.OnFinished += DoMissionFinished;

        UIInGameMain.Instance.AddDynamicMissionMsg(mission);
    }

    private void DoMissionFinished(Mission mission)
    {
        m_MissionMap.Remove(mission);
        if (MissionCount <= 0) GameMain.Instance.GameEnd();
    }
}