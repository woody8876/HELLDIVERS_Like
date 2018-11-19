using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MobAnimationsController))]
public class FishAI : Character
{
    FSMSystem m_FSM;
    public AIData m_AIData;
    public eFSMStateID m_CurrentState;
    private MobAnimationsController m_MobAnimator;
    private PlayerController m_PlayerController;
    private CapsuleCollider m_CapsuleCollider;
    private CapsuleCollider m_DamageColloder;
    private float Timer = 2.0f;

    #region Events

    public delegate void MobEventHolder();
    public event MobEventHolder OnSpawn;
    public event MobEventHolder OnDeath;

    #endregion

    // Use this for initialization
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
        m_FSM.PerformTransition(eFSMTransition.Go_Respawn);
        if (OnSpawn != null) OnSpawn();
    }
    protected override void Start()
    {
        m_AIData = new AIData();
        MobData.Instance.AIDataTable[3100].CopyTo(m_AIData);

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

        #region FSMMap
        FSMRespawnState m_RespawnState = new FSMRespawnState();
        FSMChaseState m_Chasestate = new FSMChaseState();
        FSMAttackState m_Attackstate = new FSMAttackState();
        FSMIdleState m_IdleState = new FSMIdleState();
        FSMWanderIdleState m_WanderIdleState = new FSMWanderIdleState();
        FSMWanderState m_WanderState = new FSMWanderState();

        m_RespawnState.AddTransition(eFSMTransition.Go_WanderIdle, m_WanderIdleState);

        m_Chasestate.AddTransition(eFSMTransition.Go_Attack, m_Attackstate);
        m_Chasestate.AddTransition(eFSMTransition.Go_WanderIdle, m_WanderIdleState);

        m_Attackstate.AddTransition(eFSMTransition.Go_Idle, m_IdleState);
        m_Attackstate.AddTransition(eFSMTransition.Go_Chase, m_Chasestate);
        m_Attackstate.AddTransition(eFSMTransition.Go_WanderIdle, m_WanderIdleState);

        m_IdleState.AddTransition(eFSMTransition.Go_Chase, m_Chasestate);
        m_IdleState.AddTransition(eFSMTransition.Go_Attack, m_Attackstate);
        m_IdleState.AddTransition(eFSMTransition.Go_WanderIdle, m_WanderIdleState);

        m_WanderIdleState.AddTransition(eFSMTransition.Go_Wander, m_WanderState);
        m_WanderIdleState.AddTransition(eFSMTransition.Go_Chase, m_Chasestate);

        m_WanderState.AddTransition(eFSMTransition.Go_WanderIdle, m_WanderIdleState);
        m_WanderState.AddTransition(eFSMTransition.Go_Chase, m_Chasestate);

        FSMFishGetHurtState m_GetHurtState = new FSMFishGetHurtState();
        FSMDeadState m_DeadState = new FSMDeadState();

        m_GetHurtState.AddTransition(eFSMTransition.Go_Chase, m_Chasestate);
        m_GetHurtState.AddTransition(eFSMTransition.Go_Attack, m_Attackstate);

        m_DeadState.AddTransition(eFSMTransition.Go_Respawn, m_RespawnState);

        m_FSM.AddGlobalTransition(eFSMTransition.Go_Dead, m_DeadState);
        m_FSM.AddGlobalTransition(eFSMTransition.Go_FishGetHurt, m_GetHurtState);

        m_FSM.AddState(m_RespawnState);
        m_FSM.AddState(m_WanderIdleState);
        m_FSM.AddState(m_IdleState);
        m_FSM.AddState(m_Chasestate);
        m_FSM.AddState(m_Attackstate);
        m_FSM.AddState(m_GetHurtState);
        m_FSM.AddState(m_DeadState);
        m_FSM.AddState(m_WanderState);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
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

        if (m_FSM.CurrentStateID == eFSMStateID.ChaseStateID)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(this.transform.position, m_AIData.m_vTarget);
        }
        else if (m_FSM.CurrentStateID == eFSMStateID.AttackStateID)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.transform.position, m_AIData.m_vTarget);
        }
        //Gizmos.DrawWireSphere(m_AIData.m_vTarget, 0.5f);

        Gizmos.DrawWireSphere(this.transform.position, m_AIData.m_fAttackRange);
    }
}
