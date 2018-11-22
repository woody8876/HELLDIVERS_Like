using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAI : Character
{

    private FSMSystem m_FSM;
    public MobInfo m_AIData;
    public PlayerController m_PlayerController;
    private MobAnimationsController m_MobAnimator;
    private BoxCollider m_BoxCollider;
    private BoxCollider m_DamageCollider;
    private MobAimLine m_MobAimLine;
    public bool m_bGoIdle = false;

    public eFSMStateID m_CurrentState;
    
    #region Events

    public delegate void MobEventHolder();
    public event MobEventHolder OnSpawn;
    public event MobEventHolder OnDeath;

    #endregion
    // Use this for initialization
    private void OnEnable()
    {
        if (m_FSM == null) return;
        m_bDead = false;
        m_bGoIdle = false;
        m_CurrentHp = m_MaxHp;
        m_BoxCollider.enabled = true;
        m_DamageCollider.enabled = true;
        m_FSM.PerformTransition(eFSMTransition.Go_WanderIdle);
    }
        FSMWanderIdleState m_WanderIdleState = new FSMWanderIdleState();
        FSMWanderState m_WanderState = new FSMWanderState();
        FSMCallArmyState m_CallArmyState = new FSMCallArmyState();
        FSMFleeState m_FleeState = new FSMFleeState();
        FSMChaseState m_Chasestate = new FSMChaseState();
        FSMPatrolAttackState m_PatrolAttackstate = new FSMPatrolAttackState();
        FSMIdleState m_IdleState = new FSMIdleState();
        FSMDodgeState m_DodgeState = new FSMDodgeState();
        FSMNoPlayerWanderIdleState m_FSMNoPlayerWanderIdleState = new FSMNoPlayerWanderIdleState();
        FSMNoPlayerWanderState m_FSMNoPlayerWander = new FSMNoPlayerWanderState();
    protected override void Start()
    {
        m_AIData = new MobInfo();
        GameData.Instance.MobInfoTable[3200].CopyTo(m_AIData);

        m_MaxHp = m_AIData.m_fHp;
        base.Start();

        m_MobAnimator = this.GetComponent<MobAnimationsController>();
        m_BoxCollider = this.GetComponent<BoxCollider>();
        m_DamageCollider = GetComponentInChildren<BoxCollider>();
        m_MobAimLine = this.GetComponent<MobAimLine>();
        m_FSM = new FSMSystem(m_AIData);
        m_AIData.m_Go = this.gameObject;
        m_AIData.m_FSMSystem = m_FSM;
        m_AIData.m_AnimationController = this.GetComponent<MobAnimationsController>();
        m_AIData.navMeshAgent = this.GetComponent<NavMeshAgent>();
        m_AIData.navMeshAgent.enabled = false;
        m_AIData.m_MobAimLine = m_MobAimLine;

        #region FSMMap

        #region Phase1

        m_WanderIdleState.AddTransition(eFSMTransition.Go_Wander, m_WanderState);
        m_WanderIdleState.AddTransition(eFSMTransition.Go_Flee, m_FleeState);
        m_WanderIdleState.AddTransition(eFSMTransition.Go_Idle, m_IdleState);

        m_WanderState.AddTransition(eFSMTransition.Go_WanderIdle, m_WanderIdleState);
        m_WanderState.AddTransition(eFSMTransition.Go_Flee, m_FleeState);

        m_FleeState.AddTransition(eFSMTransition.Go_CallArmy, m_CallArmyState);
        #endregion

        #region Phase2

        m_CallArmyState.AddTransition(eFSMTransition.Go_Chase, m_Chasestate);
        m_CallArmyState.AddTransition(eFSMTransition.Go_Dodge, m_DodgeState);

        m_Chasestate.AddTransition(eFSMTransition.Go_Attack, m_PatrolAttackstate);
        m_Chasestate.AddTransition(eFSMTransition.Go_NoPlayerWanderIdle, m_FSMNoPlayerWanderIdleState);

        m_DodgeState.AddTransition(eFSMTransition.Go_Attack, m_PatrolAttackstate);
        m_DodgeState.AddTransition(eFSMTransition.Go_NoPlayerWanderIdle, m_FSMNoPlayerWanderIdleState);

        m_PatrolAttackstate.AddTransition(eFSMTransition.Go_Idle, m_IdleState);
        m_PatrolAttackstate.AddTransition(eFSMTransition.Go_Chase, m_Chasestate);
        m_PatrolAttackstate.AddTransition(eFSMTransition.Go_NoPlayerWanderIdle, m_FSMNoPlayerWanderIdleState);

        m_IdleState.AddTransition(eFSMTransition.Go_Chase, m_Chasestate);
        m_IdleState.AddTransition(eFSMTransition.Go_Dodge, m_DodgeState);
        m_IdleState.AddTransition(eFSMTransition.Go_Attack, m_PatrolAttackstate);
        m_IdleState.AddTransition(eFSMTransition.Go_NoPlayerWanderIdle, m_FSMNoPlayerWanderIdleState);

        m_FSMNoPlayerWanderIdleState.AddTransition(eFSMTransition.Go_NoPlayerWander, m_FSMNoPlayerWander);
        m_FSMNoPlayerWanderIdleState.AddTransition(eFSMTransition.Go_Chase, m_Chasestate);
        m_FSMNoPlayerWanderIdleState.AddTransition(eFSMTransition.Go_Dodge, m_DodgeState);
        

        m_FSMNoPlayerWander.AddTransition(eFSMTransition.Go_NoPlayerWanderIdle, m_FSMNoPlayerWanderIdleState);
        m_FSMNoPlayerWander.AddTransition(eFSMTransition.Go_Chase, m_Chasestate);
        m_FSMNoPlayerWander.AddTransition(eFSMTransition.Go_Dodge, m_DodgeState);
        #endregion

        FSMPatrolGetHurtState m_GetHurtState = new FSMPatrolGetHurtState();
        FSMDeadState m_DeadState = new FSMDeadState();

        m_GetHurtState.AddTransition(eFSMTransition.Go_Flee, m_FleeState);
        m_GetHurtState.AddTransition(eFSMTransition.Go_Chase, m_Chasestate);
        m_GetHurtState.AddTransition(eFSMTransition.Go_Dodge, m_DodgeState);
        m_DeadState.AddTransition(eFSMTransition.Go_WanderIdle, m_WanderIdleState);

        m_FSM.AddGlobalTransition(eFSMTransition.Go_PatrolGetHurt, m_GetHurtState);
        m_FSM.AddGlobalTransition(eFSMTransition.Go_Dead, m_DeadState);

        m_FSM.AddState(m_WanderIdleState);
        m_FSM.AddState(m_WanderState);
        m_FSM.AddState(m_FleeState);
        m_FSM.AddState(m_CallArmyState);
        m_FSM.AddState(m_Chasestate);
        m_FSM.AddState(m_DodgeState);
        m_FSM.AddState(m_PatrolAttackstate);
        m_FSM.AddState(m_IdleState);
        m_FSM.AddState(m_FSMNoPlayerWanderIdleState);
        m_FSM.AddState(m_FSMNoPlayerWander);
        m_FSM.AddState(m_GetHurtState);
        m_FSM.AddState(m_DeadState);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        MobInfo.AIFunction.SearchPlayer(m_AIData);
        m_CurrentState = m_AIData.m_FSMSystem.CurrentStateID;
        if (m_bGoIdle)
        {
            m_WanderIdleState.ToIdle(m_AIData);
            m_bGoIdle = false;
        }
        else
        {
            m_FSM.DoState();
        }
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
        m_MobAimLine.CloseAimLine();
        m_FSM.PerformGlobalTransition(eFSMTransition.Go_Dead);
    }
    public override bool TakeDamage(float dmg, Vector3 hitPoint)
    {
        if (IsDead) return false;

        CurrentHp -= dmg;
        if (m_CurrentHp <= 0)
        {
            m_BoxCollider.enabled = false;
            m_DamageCollider.enabled = false;
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
        if (IsDead) return false;
        CurrentHp -= damager.Damage;
        if (m_CurrentHp <= 0)
        {
            m_BoxCollider.enabled = false;
            m_DamageCollider.enabled = false;
            Death();

            damager.Damager.Record.NumOfKills++;
            damager.Damager.Record.Exp += (int)m_AIData.m_Exp;
            damager.Damager.Record.Money += (int)m_AIData.m_Money;
            return true;
        }
        else
        {
            PerformGetHurt();
        }
        return true;
    }
    public override void Death()
    {
        m_bDead = true;
        PerformDead();
        if (OnDeath != null) OnDeath();
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

        Gizmos.DrawWireSphere(this.transform.position, m_AIData.m_fPatrolVisionLength);
    }
}
