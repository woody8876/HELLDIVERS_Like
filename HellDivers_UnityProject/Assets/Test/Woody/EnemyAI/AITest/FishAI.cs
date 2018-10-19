using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class FishAI : MonoBehaviour {

    public AIData m_Data;
    FSMSystem m_Fsm;
	// Use this for initialization
	void Start () {
        m_Fsm = new FSMSystem(m_Data);
        m_Data.m_Go = this.gameObject;
        m_Data.m_FSMSystem = m_Fsm;
        m_Data.navMeshAgent = GetComponent<NavMeshAgent>();

        FSMMoveToState mtstate = new FSMMoveToState();
        FSMChaseState chasestate = new FSMChaseState();
        FSMAttackState attackstate = new FSMAttackState();

        mtstate.AddTransition(eFSMTransition.Go_Chase, chasestate);
        chasestate.AddTransition(eFSMTransition.Go_Attack, attackstate);
        attackstate.AddTransition(eFSMTransition.Go_Chase, chasestate);

        m_Fsm.AddState(mtstate);
        //m_Fsm.AddState(chasestate);
        //m_Fsm.AddState(attackstate);
    }
	
	// Update is called once per frame
	void Update () {
        if (m_Data.m_PlayerGO == null)
        {
            m_Data.m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
        }
        m_Fsm.DoState();
    }
    private void OnDrawGizmos()
    {
        if (m_Data == null || m_Fsm == null)
        {
            return;
        }
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2.0f);
        if (m_Fsm.CurrentStateID == eFSMStateID.MoveToStateID)
        {
            Gizmos.color = Color.green;
        }
        else if (m_Fsm.CurrentStateID == eFSMStateID.ChaseStateID)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(this.transform.position, m_Data.m_vTarget);
        }
        else if (m_Fsm.CurrentStateID == eFSMStateID.AttackStateID)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.transform.position, m_Data.m_vTarget);
        }
        Gizmos.DrawWireSphere(m_Data.m_vTarget, 0.5f);

        Gizmos.DrawWireSphere(this.transform.position, m_Data.m_fAttackRange);
    }
}
