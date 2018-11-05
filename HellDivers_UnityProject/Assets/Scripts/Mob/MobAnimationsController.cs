using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAnimationsController : MonoBehaviour {

    public Animator Animator { get { return m_Animator; } set { m_Animator = value; } }

    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = this.GetComponent<Animator>();
    }
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetAnimator(eFSMStateID state)
    {
        UpdateAnimator(state);
    }

    public void SetAnimator(eFSMStateID state, bool Bool)
    {
        UpdateAnimator(state, Bool);
    }

    private void UpdateAnimator(eFSMStateID state)
    {
        switch (state)
        {
            case eFSMStateID.WanderIdleStateID:
                m_Animator.SetTrigger("WanderIdle");
                break;
            case eFSMStateID.AttackStateID:
                m_Animator.SetTrigger("Attack");
                break;
            case eFSMStateID.CallArmyState:
                m_Animator.SetTrigger("CallArmy");
                break;
            case eFSMStateID.FishGetHurtStateID:
                m_Animator.SetTrigger("GetHurt");
                break;
            case eFSMStateID.PatrolGetHurtID:
                m_Animator.SetTrigger("GetHurt");
                break;
            case eFSMStateID.DeadStateID:
                m_Animator.SetTrigger("Dead");
                break;
        }
    }

    private void UpdateAnimator(eFSMStateID state, bool Bool)
    {
        switch (state)
        {
            case eFSMStateID.ChaseStateID:
                m_Animator.SetBool("Chase", Bool);
                break;
            case eFSMStateID.IdleStateID:
                m_Animator.SetBool("Idle", Bool);
                break;
            case eFSMStateID.WanderStateID:
                m_Animator.SetBool("Wander", Bool);
                break;
            case eFSMStateID.WanderIdleStateID:
                m_Animator.SetBool("WanderIdle", Bool);
                break;
            case eFSMStateID.FleeStateID:
                m_Animator.SetBool("Flee", Bool);
                break;
        }
    }
}
