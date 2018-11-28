using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HELLDIVERS.UI;

public class UIMissionInfo : MonoBehaviour
{
    [SerializeField] private Image m_CheckMark;
    [SerializeField] private Image m_Icon;
    [SerializeField] private Text m_Description;
    [SerializeField] private Text m_Count;
    private eMissionType m_Type;
    private LinkedList<Mission> m_Missions = new LinkedList<Mission>();

    public void Initialize(Mission mission)
    {
        m_Type = mission.Type;
        SetIcon(mission.Type);
        m_Missions.AddFirst(mission);
        SubscribeEvents(mission);
        RefreshCount();
    }

    public void AddMission(Mission mission)
    {
        if (mission.Type != m_Type) return;

        m_Missions.AddLast(mission);
        SubscribeEvents(mission);
        RefreshCount();
    }

    private void SetIcon(eMissionType type)
    {
        switch (type)
        {
            case eMissionType.Tower:
                Sprite icon = ResourceManager.m_Instance.LoadSprite(typeof(Sprite), UIHelper.MissionIconFolder, "MissionTower");
                m_Icon.sprite = icon;
                break;
        }
    }

    private void SubscribeEvents(Mission mission)
    {
        mission.OnMissionComplete += RefreshCount;
    }

    private void UnsbscribeEvents(Mission mission)
    {
        mission.OnMissionComplete += RefreshCount;
    }

    private void RefreshCount()
    {
        int completeCount = 0;
        foreach (Mission mission in m_Missions)
        {
            if (mission.IsFinished) completeCount++;
        }
        m_Count.text = string.Format("( {0} / {1} )", completeCount, m_Missions.Count); ;

        if (completeCount == m_Missions.Count)
        {
            m_CheckMark.gameObject.SetActive(true);
            m_Description.color = UIHelper.Player1_Color;
            m_Count.color = UIHelper.Player1_Color;
        }
    }

    private void OnDestroy()
    {
        foreach (Mission mission in m_Missions)
        {
            UnsbscribeEvents(mission);
        }
    }
}