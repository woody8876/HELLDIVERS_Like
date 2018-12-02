using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionBriefingIntroduction : MonoBehaviour {

    [SerializeField] private Transform m_UIRoot;
    [SerializeField] private UIMissionItem m_TowerMission;
    [SerializeField] private UIMissionItem m_KillMission;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddMissionType(eMissionType type)
    {
        UIMissionItem item = null;
        switch (type)
        {
            case eMissionType.Tower:
                item = Instantiate(m_TowerMission, m_UIRoot);
                break;
            case eMissionType.KillMob:
                item = Instantiate(m_KillMission, m_UIRoot);
                break;
        }
    }
}
