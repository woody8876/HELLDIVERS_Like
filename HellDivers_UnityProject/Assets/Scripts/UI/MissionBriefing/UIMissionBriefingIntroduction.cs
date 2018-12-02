using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using HELLDIVERS.UI.InGame;

public class UIMissionBriefingIntroduction : MonoBehaviour {

    [SerializeField] private Transform m_UIRoot;
    [SerializeField] private UIMissionGroupInfo m_MissionGroupInfoPrefab;
    [SerializeField] private UIMissionCountInfo m_MissionCountInfoPrefab;
    [SerializeField] private Text m_Introduction;

    private Dictionary<eMissionType, List<UIMissionInfo>> m_MissionElementMap = new Dictionary<eMissionType, List<UIMissionInfo>>();

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
                    UIMissionGroupInfo missionGroupUI = Instantiate(m_MissionGroupInfoPrefab, m_UIRoot);
                    missionGroupUI.Initialize(mission);
                    pList.Add(missionGroupUI);
                }

                break;

            case eMissionType.KillMob:
                UIMissionCountInfo missionCountUI = Instantiate(m_MissionCountInfoPrefab, m_UIRoot);
                missionCountUI.Initialize(mission);
                pList.Add(missionCountUI);
                break;
        }

        if (m_MissionElementMap.Count == 1) EventSystem.current.SetSelectedGameObject(m_MissionElementMap[mission.Type][0].gameObject);
    }
}
