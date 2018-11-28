using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelMission : MonoBehaviour
{
    public static UIPanelMission Instance { get; private set; }

    [SerializeField] private UIMissionGroupInfo m_MissionGroupInfoPrefab;
    [SerializeField] private UIMissionCountInfo m_MissionCountInfoPrefab;

    private Dictionary<eMissionType, List<UIMissionInfo>> m_MissionElementMap;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        m_MissionElementMap = new Dictionary<eMissionType, List<UIMissionInfo>>();
    }

    public void AddMissionInfo(Mission mission)
    {
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

        switch (mission.Type)
        {
            case eMissionType.Tower:

                if (pList.Count > 0)
                {
                    UIMissionGroupInfo missionGroupUI = pList[0] as UIMissionGroupInfo;
                    missionGroupUI.AddMission(mission);
                }
                else
                {
                    UIMissionGroupInfo missionGroupUI = Instantiate(m_MissionGroupInfoPrefab, this.transform);
                    missionGroupUI.Initialize(mission);
                    pList.Add(missionGroupUI);
                }

                break;

            case eMissionType.KillMob:
                UIMissionCountInfo missionCountUI = Instantiate(m_MissionCountInfoPrefab, this.transform);
                missionCountUI.Initialize(mission);
                pList.Add(missionCountUI);
                break;
        }
    }
}