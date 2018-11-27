using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HELLDIVERS.UI.InGame;

public enum eMissionType
{
    Tower,
    Escape,
}

public class MissionFactory
{
    public Mission CreateMission(eMissionType type)
    {
        Mission mission = null;

        switch (type)
        {
            case eMissionType.Tower:
                GameObject tower = new GameObject("MissionTower");
                MissionTower missionTower = tower.AddComponent<MissionTower>();

                MissionTowerData data = ResourceManager.m_Instance.LoadData(typeof(MissionTowerData), "Mission/Settings", "MissionTowerSetting") as MissionTowerData;
                missionTower.Initialize(data);
                mission = missionTower;

                UIInGameMain.Instance.AddDynamicMissionMsg(missionTower);
                UIInGameMain.Instance.AddMapPoint(tower, eMapPointType.MISSIONTOWER);
                break;
        }

        return mission;
    }
}