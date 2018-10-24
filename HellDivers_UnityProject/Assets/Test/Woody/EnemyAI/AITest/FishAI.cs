using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MobAnimationsController))]
public class FishAI : MonoBehaviour {
    
    FSMSystem m_Fsm;
    private MobAnimationsController m_MAC;
    public AIData m_AIData;
    // Use this for initialization
    private void Awake()
    {
        
    }
    void Start () {

        m_MAC = this.GetComponent<MobAnimationsController>();

        m_AIData = new AIData();
        m_Fsm = new FSMSystem(m_AIData);
        m_AIData.m_Go = this.gameObject;
        m_AIData.m_FSMSystem = m_Fsm;
        m_AIData.m_AnimationController = this.GetComponent<MobAnimationsController>();
        m_AIData.navMeshAgent = this.GetComponent<NavMeshAgent>();

        FSMMoveToState mtstate = new FSMMoveToState();
        FSMChaseState chasestate = new FSMChaseState();
        FSMAttackState attackstate = new FSMAttackState();

        mtstate.AddTransition(eFSMTransition.Go_Chase, chasestate);

        chasestate.AddTransition(eFSMTransition.Go_Attack, attackstate);

        attackstate.AddTransition(eFSMTransition.Go_Chase, chasestate);

        m_Fsm.AddState(mtstate);
        m_Fsm.AddState(chasestate);
        m_Fsm.AddState(attackstate);
    }
	
	// Update is called once per frame
	void Update () {
        if (m_AIData.m_PlayerGO == null)
        {
            m_AIData.m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
        }
        m_Fsm.DoState();
    }
    private void OnDrawGizmos()
    {
        if (m_AIData == null || m_Fsm == null)
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
            Gizmos.DrawLine(this.transform.position, m_AIData.m_vTarget);
        }
        else if (m_Fsm.CurrentStateID == eFSMStateID.AttackStateID)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.transform.position, m_AIData.m_vTarget);
        }
        Gizmos.DrawWireSphere(m_AIData.m_vTarget, 0.5f);

        Gizmos.DrawWireSphere(this.transform.position, m_AIData.m_fAttackRange);
    }
}
