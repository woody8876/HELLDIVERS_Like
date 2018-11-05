using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAI : Character{

    public AIData m_AIData;
    public PlayerController m_PlayerController;
    FSMSystem m_FSM;
    private MobAnimationsController m_MobAnimator;

    // Use this for initialization
    private void OnEnable()
    {
        m_bDead = false;
        m_CurrentHp = m_MaxHp;
    }
    protected override void Start() {
        m_MaxHp = 450;
        base.Start();

        m_MobAnimator = this.GetComponent<MobAnimationsController>();
        m_AIData = new AIData();
        m_FSM = new FSMSystem(m_AIData);
        m_AIData.m_ID = 3002;
        m_AIData.m_Go = this.gameObject;
        m_AIData.m_FSMSystem = m_FSM;
        m_AIData.m_AnimationController = this.GetComponent<MobAnimationsController>();
        m_AIData.navMeshAgent = this.GetComponent<NavMeshAgent>();
        m_AIData.navMeshAgent.enabled = false;

        #region FSMMap
        FSMWanderIdleState m_WanderIdleState = new FSMWanderIdleState();
        FSMWanderState m_WanderState = new FSMWanderState();
        FSMCallArmyState m_CallArmyState = new FSMCallArmyState();
        FSMFleeState m_FleeState = new FSMFleeState();

        m_WanderIdleState.AddTransition(eFSMTransition.GO_Wander, m_WanderState);
        m_WanderIdleState.AddTransition(eFSMTransition.GO_Flee, m_FleeState);

        m_WanderState.AddTransition(eFSMTransition.GO_WanderIdle, m_WanderIdleState);
        m_WanderState.AddTransition(eFSMTransition.GO_Flee, m_FleeState);

        m_FleeState.AddTransition(eFSMTransition.Go_CallArmy, m_CallArmyState);

        m_CallArmyState.AddTransition(eFSMTransition.GO_Flee, m_FleeState);
        m_CallArmyState.AddTransition(eFSMTransition.GO_WanderIdle, m_WanderIdleState);

        FSMPatrolGetHurtState m_GetHurtState = new FSMPatrolGetHurtState();
        FSMDeadState m_DeadState = new FSMDeadState();

        m_GetHurtState.AddTransition(eFSMTransition.GO_Flee, m_FleeState);
        m_DeadState.AddTransition(eFSMTransition.GO_WanderIdle, m_WanderIdleState);

        m_FSM.AddGlobalTransition(eFSMTransition.Go_PatrolGetHurt, m_GetHurtState);
        m_FSM.AddGlobalTransition(eFSMTransition.Go_Dead, m_DeadState);

        m_FSM.AddState(m_WanderIdleState);
        m_FSM.AddState(m_WanderState);
        m_FSM.AddState(m_GetHurtState);
        m_FSM.AddState(m_CallArmyState);
        m_FSM.AddState(m_FleeState);
        m_FSM.AddState(m_DeadState);
        #endregion
    }

    // Update is called once per frame
    void Update(){
        if (m_AIData.m_PlayerGO == null)
        {
            m_AIData.m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
            m_PlayerController = m_AIData.m_PlayerGO.GetComponent<PlayerController>();
        }

        m_FSM.DoState();
        
        if (Input.GetKeyDown(KeyCode.U)) Death();
    }

    public override bool TakeDamage(float dmg, Vector3 hitPoint)
    {
        if (IsDead) return false;

        CurrentHp -= dmg;
        if (m_CurrentHp <= 0)
        {
            Death();
            return true;
        }
        else
        {
            PerformGetHurt();
        }
        return true;
    }
    public override bool TakeDamage(IDamager damager, Vector3 hitPoint)
    {
        return TakeDamage(damager.Damage, hitPoint);
    }

    public void PerformGetHurt()
    {
        if (IsDead) return;

        AnimatorStateInfo info = m_MobAnimator.Animator.GetCurrentAnimatorStateInfo(0);
        if (m_MobAnimator.Animator.IsInTransition(0) || info.IsName("GetHurt"))
        {
            return;
        }
        m_FSM.PerformGlobalTransition(eFSMTransition.Go_PatrolGetHurt);
        return;
    }
    public void PerformDead()
    {
        m_MobAnimator.Animator.ResetTrigger("GetHurt");
        m_FSM.PerformGlobalTransition(eFSMTransition.Go_Dead);
    }

    public override void Death()
    {
        m_bDead = true;
        PerformDead();
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
