using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameRewardManager : MonoBehaviour
{
    static public InGameRewardManager Instance { get { return m_instance; } }
    static private InGameRewardManager m_instance;

    public Dictionary<int, PlayerRecord> PlayerRewardMap { get { return m_RecordMap; } }
    private Dictionary<int, PlayerRecord> m_RecordMap;
    private PlayerRecord record1;

    private void Awake()
    {
        if (m_instance == null) m_instance = this;
        else Destroy(this.gameObject);
    }

    // Use this for initialization
    private void Start()
    {
        m_RecordMap = new Dictionary<int, PlayerRecord>();
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    private void Update()
    {
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

        record1 = record;
    }
}