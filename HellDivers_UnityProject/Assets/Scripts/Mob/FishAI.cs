using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MobAnimationsController))]
public class FishAI : Character {
    
    FSMSystem m_Fsm;
    public AIData m_AIData;
    // Use this for initialization
    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        
    }
    protected override void Start () {
        base.Start();
        m_AIData = new AIData();
        m_Fsm = new FSMSystem(m_AIData);
        m_AIData.m_Go = this.gameObject;
        m_AIData.m_FSMSystem = m_Fsm;
        m_AIData.m_AnimationController = this.GetComponent<MobAnimationsController>();
        m_AIData.navMeshAgent = this.GetComponent<NavMeshAgent>();
        m_AIData.navMeshAgent.enabled = false;

        FSMMoveToState m_MoveState = new FSMMoveToState();
        FSMChaseState m_Chasestate = new FSMChaseState();
        FSMAttackState m_Attackstate = new FSMAttackState();
        FSMIdleState m_IdleState = new FSMIdleState();

        m_MoveState.AddTransition(eFSMTransition.Go_Chase, m_Chasestate);

        m_Chasestate.AddTransition(eFSMTransition.Go_Attack, m_Attackstate);
        
        m_Attackstate.AddTransition(eFSMTransition.Go_Idle, m_IdleState);

        m_IdleState.AddTransition(eFSMTransition.Go_Chase, m_Chasestate);
        m_IdleState.AddTransition(eFSMTransition.Go_Attack, m_Attackstate);

        FSMDeadState m_DeadState = new FSMDeadState();
        m_DeadState.AddTransition(eFSMTransition.Go_MoveTo, m_MoveState);

        m_Fsm.AddGlobalTransition(eFSMTransition.Go_Dead, m_DeadState);

        m_Fsm.AddState(m_MoveState);
        m_Fsm.AddState(m_Chasestate);
        m_Fsm.AddState(m_Attackstate);
        m_Fsm.AddState(m_DeadState);
    }
	
	// Update is called once per frame
	void Update () {
        if (m_AIData.m_PlayerGO == null)
        {
            m_AIData.m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
        }
        m_Fsm.DoState();
        if (Input.GetKeyDown(KeyCode.U))
        {
            m_Fsm.PerformGlobalTransition(eFSMTransition.Go_Dead);
        }
    }
    public override void Death()
    {
        m_bDead = true;
        m_Fsm.PerformGlobalTransition(eFSMTransition.Go_Dead);
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
