using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankAI : Character
{
    FSMSystem m_FSM;
    public AIData m_AIData;
    private MobAnimationsController m_MobAnimator;
    private PlayerController m_PlayerController;
    private CapsuleCollider m_CapsuleCollider;
    private CapsuleCollider m_DamageColloder;
    //private GameObject[] m_PlayerGO;
    private float m_MinDis = 100000f;
    private float Timer = 2.0f;

    public eFSMStateID m_CurrentState;

    private void Awake()
    {
    }
    private void OnEnable()
    {
        if (m_FSM == null) return;
        m_AIData.m_Go = this.gameObject;
        m_bDead = false;
        m_CurrentHp = m_MaxHp;
        m_CapsuleCollider.enabled = true;
        m_DamageColloder.enabled = true;
        m_MinDis = 100000f;
        m_FSM.PerformTransition(eFSMTransition.Go_Respawn);
    }
    protected override void Start()
    {
        m_AIData = new AIData();
        MobData.Instance.AIDataTable[3400].CopyTo(m_AIData);

        m_MaxHp = m_AIData.m_fHp;
        base.Start();

        m_MobAnimator = this.GetComponent<MobAnimationsController>();
        m_CapsuleCollider = this.GetComponent<CapsuleCollider>();
        m_DamageColloder = GetComponentInChildren<CapsuleCollider>();
        m_FSM = new FSMSystem(m_AIData);
        m_AIData.m_Go = this.gameObject;
        m_AIData.m_FSMSystem = m_FSM;
        m_AIData.m_AnimationController = this.GetComponent<MobAnimationsController>();
        m_AIData.navMeshAgent = this.GetComponent<NavMeshAgent>();
        m_AIData.navMeshAgent.speed = Random.Range(4.5f, 5.0f);
        m_AIData.navMeshAgent.enabled = false;

        FSMRespawnState m_RespawnState = new FSMRespawnState();
        FSMChaseToRemoteAttackState m_ChaseToRemoteAttackState = new FSMChaseToRemoteAttackState();
        FSMChaseState m_ChaseState = new FSMChaseState();
        FSMAttackState m_Attackstate = new FSMAttackState();
        FSMRemoteAttackState m_RemoteAttackstate = new FSMRemoteAttackState();
        FSMIdleState m_IdleState = new FSMIdleState();
        FSMWanderIdleState m_WanderIdleState = new FSMWanderIdleState();
        FSMWanderState m_WanderState = new FSMWanderState();

        m_RespawnState.AddTransition(eFSMTransition.Go_WanderIdle, m_WanderIdleState);

        m_ChaseToRemoteAttackState.AddTransition(eFSMTransition.Go_RemoteAttack, m_RemoteAttackstate);
        m_ChaseToRemoteAttackState.AddTransition(eFSMTransition.Go_WanderIdle, m_WanderIdleState);

        m_RemoteAttackstate.AddTransition(eFSMTransition.Go_Idle, m_IdleState);

        m_IdleState.AddTransition(eFSMTransition.Go_ChaseToRemoteAttack, m_ChaseToRemoteAttackState);
        m_IdleState.AddTransition(eFSMTransition.Go_RemoteAttack, m_RemoteAttackstate);
        m_IdleState.AddTransition(eFSMTransition.Go_Chase, m_ChaseState);
        m_IdleState.AddTransition(eFSMTransition.Go_Attack, m_Attackstate);
        m_IdleState.AddTransition(eFSMTransition.Go_WanderIdle, m_WanderIdleState);

        m_ChaseState.AddTransition(eFSMTransition.Go_Attack, m_Attackstate);
        m_ChaseState.AddTransition(eFSMTransition.Go_WanderIdle, m_WanderIdleState);

        m_Attackstate.AddTransition(eFSMTransition.Go_Idle, m_IdleState);
        
        m_WanderIdleState.AddTransition(eFSMTransition.Go_ChaseToRemoteAttack, m_ChaseToRemoteAttackState);
        m_WanderIdleState.AddTransition(eFSMTransition.Go_Wander, m_WanderState);

        FSMTankGetHurtState m_GetHurtState = new FSMTankGetHurtState();
        FSMDeadState m_DeadState = new FSMDeadState();

        m_GetHurtState.AddTransition(eFSMTransition.Go_ChaseToRemoteAttack, m_ChaseToRemoteAttackState);
        m_GetHurtState.AddTransition(eFSMTransition.Go_RemoteAttack, m_RemoteAttackstate);

        m_DeadState.AddTransition(eFSMTransition.Go_Respawn, m_RespawnState);

        m_FSM.AddGlobalTransition(eFSMTransition.Go_Dead, m_DeadState);
        m_FSM.AddGlobalTransition(eFSMTransition.Go_FishGetHurt, m_GetHurtState);


        m_FSM.AddState(m_RespawnState);
        m_FSM.AddState(m_ChaseToRemoteAttackState);
        m_FSM.AddState(m_ChaseState);
        m_FSM.AddState(m_Attackstate);
        m_FSM.AddState(m_RemoteAttackstate);
        m_FSM.AddState(m_IdleState);
        m_FSM.AddState(m_WanderIdleState);
        m_FSM.AddState(m_WanderState);
        m_FSM.AddState(m_GetHurtState);
        m_FSM.AddState(m_DeadState);
    }

    // Update is called once per frame
    void Update () {
        Timer += Time.deltaTime;

        if (Timer > 2.0f)
        {
            AIData.AIFunction.SearchPlayer(m_AIData);
            Timer = 0.0f;
            return;
        }
        m_CurrentState = m_AIData.m_FSMSystem.CurrentStateID;
        m_FSM.DoState();

        if (Input.GetKeyDown(KeyCode.U)) Death();
    }

    public void PerformGetHurt()
    {
        if (IsDead) return;
        AnimatorStateInfo info = m_MobAnimator.Animator.GetCurrentAnimatorStateInfo(0);
        if (m_MobAnimator.Animator.IsInTransition(0) || info.IsName("GetHurt"))
        {
            return;
        }
        m_FSM.PerformGlobalTransition(eFSMTransition.Go_FishGetHurt);
        return;
    }

    public void PerformDead()
    {
        m_MobAnimator.Animator.ResetTrigger("GetHurt");
        m_FSM.PerformGlobalTransition(eFSMTransition.Go_Dead);
    }

    public override bool TakeDamage(float dmg, Vector3 hitPoint)
    {
        if (IsDead) return false;
        CurrentHp -= dmg;
        if (m_CurrentHp <= 0)
        {
            m_CapsuleCollider.enabled = false;
            m_DamageColloder.enabled = false;
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

}
