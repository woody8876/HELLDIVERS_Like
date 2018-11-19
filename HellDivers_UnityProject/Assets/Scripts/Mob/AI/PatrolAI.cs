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
    //private GameObject[] m_PlayerGO;
    private float m_MinDis = 10000000f;
    private float Timer = 0.0f;

    public eFSMStateID m_CurrentState;

    // Use this for initialization
    private void OnEnable()
    {
        if (m_FSM == null) return;
        m_bDead = false;
        m_CurrentHp = m_MaxHp;
        m_BoxCollider.enabled = true;
        m_DamageCollider.enabled = true;
    }
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
        FSMWanderIdleState m_WanderIdleState = new FSMWanderIdleState();
        FSMWanderState m_WanderState = new FSMWanderState();
        FSMCallArmyState m_CallArmyState = new FSMCallArmyState();
        FSMFleeState m_FleeState = new FSMFleeState();

        m_WanderIdleState.AddTransition(eFSMTransition.Go_Wander, m_WanderState);
        m_WanderIdleState.AddTransition(eFSMTransition.Go_Flee, m_FleeState);

        m_WanderState.AddTransition(eFSMTransition.Go_WanderIdle, m_WanderIdleState);
        m_WanderState.AddTransition(eFSMTransition.Go_Flee, m_FleeState);

        m_FleeState.AddTransition(eFSMTransition.Go_CallArmy, m_CallArmyState);
        #endregion

        #region Phase2
        FSMChaseState m_Chasestate = new FSMChaseState();
        FSMPatrolAttackState m_PatrolAttackstate = new FSMPatrolAttackState();
        FSMIdleState m_IdleState = new FSMIdleState();
        FSMDodgeState m_DodgeState = new FSMDodgeState();
        FSMNoPlayerWanderIdleState m_FSMNoPlayerWanderIdleState = new FSMNoPlayerWanderIdleState();
        FSMNoPlayerWanderState m_FSMNoPlayerWander = new FSMNoPlayerWanderState();

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
        m_FSM.DoState();
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
        return TakeDamage(damager.Damage, hitPoint);
    }

    public override void Death()
    {
        m_bDead = true;
        PerformDead();
    }

    private void Init()
    {
        m_AIData.m_ID = MobData.Instance.AIDataTable[3200].m_ID;
        m_AIData.m_fHp = MobData.Instance.AIDataTable[3200].m_fHp;
        m_AIData.m_fProbeLength = MobData.Instance.AIDataTable[3200].m_fProbeLength;
        m_AIData.m_fSight = MobData.Instance.AIDataTable[3200].m_fSight;
        m_AIData.m_fRadius = MobData.Instance.AIDataTable[3200].m_fRadius;
        m_AIData.m_fPatrolVisionLength = MobData.Instance.AIDataTable[3200].m_fPatrolVisionLength;
        m_AIData.m_fAttackRange = MobData.Instance.AIDataTable[3200].m_fAttackRange;
        m_AIData.m_fAttackDamage = MobData.Instance.AIDataTable[3200].m_fAttackDamage;
        m_AIData.m_Money = MobData.Instance.AIDataTable[3200].m_Money;
        m_AIData.m_Exp = MobData.Instance.AIDataTable[3200].m_Exp;
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
