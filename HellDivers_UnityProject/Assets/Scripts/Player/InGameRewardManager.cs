using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameRewardManager : MonoBehaviour
{
    static public InGameRewardManager Instance { get { return m_instance; } }
    static private InGameRewardManager m_instance;

    public float GameDurationTime { get { return m_GameDurationTime; } }
    public Dictionary<int, PlayerRecord> PlayerRewardMap { get { return m_RecordMap; } }
    private Dictionary<int, PlayerRecord> m_RecordMap;
    private Dictionary<eMissionType, List<MissionReward>> m_MissionRewardMap;
    private float m_GameDurationTime;

    private void Awake()
    {
        if (m_instance != null)
        {
            Destroy(m_instance.gameObject);
        }

        m_instance = this;
    }

    // Use this for initialization
    private void Start()
    {
        m_RecordMap = new Dictionary<int, PlayerRecord>();
        m_MissionRewardMap = new Dictionary<eMissionType, List<MissionReward>>();
        DontDestroyOnLoad(this);
    }

    public void SetReward(int SerialNum, PlayerRecord record)
    {
        if (m_RecordMap.ContainsKey(SerialNum))
        {
            m_RecordMap[SerialNum] = record;
        }
        else
        {
            m_RecordMap.Add(SerialNum, record);
        }
    }

    public void SetGameDurationTime(float time)
    {
        m_GameDurationTime = time;
    }

    public void SetMissionReward(eMissionType type, MissionReward reward)
    {
        List<MissionReward> pList;
        if (m_MissionRewardMap.ContainsKey(type))
        {
            pList = m_MissionRewardMap[type];
        }
        else
        {
            pList = new List<MissionReward>();
            m_MissionRewardMap.Add(type, pList);
        }

        pList.Add(reward);
    }

    public MissionReward CaculateAllMissionReward()
    {
        MissionReward reward = new MissionReward();

        foreach (var missions in m_MissionRewardMap)
        {
            foreach (var missionReward in missions.Value)
            {
                reward.EXP += missionReward.EXP;
                reward.Money += missionReward.Money;
            }
        }

        return reward;
    }

    public void ApplyRewardToPlayers()
    {
        foreach (var player in PlayerManager.Instance.Players)
        {
            PlayerInfo info = player.Value.info;
            PlayerRecord record = m_RecordMap[player.Key];

            info.AddExp(record.Exp);
            info.AddMoney(record.Money);

            MissionReward missionReward = CaculateAllMissionReward();
            info.AddExp(missionReward.EXP);
            info.AddMoney(missionReward.Money);
        }

        Destroy(this.gameObject);
    }
}