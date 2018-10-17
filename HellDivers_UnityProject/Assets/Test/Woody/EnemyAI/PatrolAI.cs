using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour {

    public AIData m_Data;
    FSMSystem m_FSM;
    // Use this for initialization
    void Start () {
        m_FSM = new FSMSystem(m_Data);
        m_Data.m_Go = this.gameObject;
        m_Data.m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
        m_Data.m_FSMSystem = m_FSM;

        FSMWanderIdleState wanderIdleState = new FSMWanderIdleState();
        FSMWanderState wanderState = new FSMWanderState();
        FSMFleeState fleeState = new FSMFleeState();

        wanderIdleState.AddTransition(eFSMTransition.GO_Wander, wanderState);
        wanderIdleState.AddTransition(eFSMTransition.GO_Flee, fleeState);

        wanderState.AddTransition(eFSMTransition.GO_WanderIdle, wanderIdleState);
        wanderState.AddTransition(eFSMTransition.GO_Flee, fleeState);

        fleeState.AddTransition(eFSMTransition.GO_WanderIdle, wanderIdleState);

        m_FSM.AddState(wanderIdleState);
        m_FSM.AddState(wanderState);
        m_FSM.AddState(fleeState);
    }
	
	// Update is called once per frame
	void Update () {
        if(m_Data.m_PlayerGO == null)
        {
            m_Data.m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
        }
        m_FSM.DoState();
	}
    private void OnDrawGizmos()
    {
        if (m_Data == null || m_FSM == null)
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
            Gizmos.DrawLine(this.transform.position, m_Data.m_vTarget);
        }
        Gizmos.DrawWireSphere(m_Data.m_vTarget, 0.5f);

        Gizmos.DrawWireSphere(this.transform.position, m_Data.m_fProbeLength);
    }
}
