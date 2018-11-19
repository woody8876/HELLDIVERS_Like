using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeekTest : MonoBehaviour {

    public MobInfo m_Data;
    FSMSystem m_FSM;
    public GameObject m_Target;
    // Use this for initialization
    void Start () {
        m_FSM = new FSMSystem(m_Data);
        m_Data.m_Go = this.gameObject;
        m_Data.m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
        m_Data.navMeshAgent = GetComponent<NavMeshAgent>();
        m_Data.m_FSMSystem = m_FSM;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_Data.m_PlayerGO == null)
        {
            m_Data.m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
        }
        m_Data.m_vTarget = m_Data.m_PlayerGO.transform.position;
        SteeringBehaviours.Seek(m_Data);
        SteeringBehaviours.NavMove(m_Data);
    }
}
