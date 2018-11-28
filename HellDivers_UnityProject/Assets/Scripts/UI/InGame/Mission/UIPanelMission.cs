using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelMission : MonoBehaviour
{
    public static UIPanelMission Instance { get; private set; }

    [SerializeField] private UIMissionInfo m_MissionInfoPrefab;

    private Dictionary<eMissionType, List<UIMissionInfo>> m_MissionElementMap;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        m_MissionElementMap = new Dictionary<eMissionType, List<UIMissionInfo>>();
    }

    public void AddMissionInfo(Mission mission)
    {
        switch (mission.Type)
        {
            case eMissionType.Tower:

                List<UIMissionInfo> pList;
                if (m_MissionElementMap.ContainsKey(mission.Type))
                {
                    pList = m_MissionElementMap[mission.Type];
                }
                else
                {
                    pList = new List<UIMissionInfo>();
                    m_MissionElementMap.Add(mission.Type, pList);
                }

                UIMissionInfo missionUI;
                if (pList.Count > 0)
                {
                    missionUI = pList[0];
                    missionUI.AddMission(mission);
                }
                else
                {
                    missionUI = Instantiate(m_MissionInfoPrefab, this.transform);
                    missionUI.Initialize(mission);
                    pList.Add(missionUI);
                }

                break;
        }
    }
}