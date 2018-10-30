using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAI : MonoBehaviour {

    public AIData m_AIData;
    FSMSystem m_FSM;
    // Use this for initialization
    void Start () {
        m_FSM = new FSMSystem(m_AIData);
        m_AIData.m_Go = this.gameObject;
        m_AIData.m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
        m_AIData.m_FSMSystem = m_FSM;
        m_AIData.m_AnimationController = this.GetComponent<MobAnimationsController>();
        m_AIData.navMeshAgent = this.GetComponent<NavMeshAgent>();
        m_AIData.navMeshAgent.enabled = false;

        FSMWanderIdleState m_WanderIdleState = new FSMWanderIdleState();
        FSMWanderState m_WanderState = new FSMWanderState();
        FSMCallArmyState m_CallArmyState = new FSMCallArmyState();
        FSMFleeState m_FleeState = new FSMFleeState();

        m_WanderIdleState.AddTransition(eFSMTransition.GO_Wander, m_WanderState);
        m_WanderIdleState.AddTransition(eFSMTransition.Go_CallArmy, m_CallArmyState);

        m_WanderState.AddTransition(eFSMTransition.GO_WanderIdle, m_WanderIdleState);
        m_WanderState.AddTransition(eFSMTransition.Go_CallArmy, m_CallArmyState);

        m_CallArmyState.AddTransition(eFSMTransition.GO_Flee, m_FleeState);

        m_FleeState.AddTransition(eFSMTransition.GO_WanderIdle, m_WanderIdleState);

        m_FSM.AddState(m_WanderIdleState);
        m_FSM.AddState(m_WanderState);
        m_FSM.AddState(m_CallArmyState);
        m_FSM.AddState(m_FleeState);
    }
	
	// Update is called once per frame
	void Update () {
        if(m_AIData.m_PlayerGO == null)
        {
            m_AIData.m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
        }
        m_FSM.DoState();
	}
    private void OnDrawGizmos()
    {
        if (m_AIData == null || m_FSM == null)
        {
            return;
        }
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2.0f);
        if (m_FSM.CurrentStateID == eFSMStateID.WanderStateID)
        {
            Gizmos.color = Color.green;
        }
        else if (m_FSM.CurrentStateID == eFSMStateID.FleeStateID)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(this.transform.position, m_AIData.m_vTarget);
        }
        Gizmos.DrawWireSphere(m_AIData.m_vTarget, 0.5f);

        Gizmos.DrawWireSphere(this.transform.position, m_AIData.m_fProbeLength);
    }
}
