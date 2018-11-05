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
        if(state == eFSMStateID.MoveToStateID)
        {
           // m_Animator.SetBool();
        }
        else if (state == eFSMStateID.FishGetHurtStateID)
        {
            m_Animator.SetTrigger("GetHurt");
        }
        else if (state == eFSMStateID.PatrolGetHurtID)
        {
            m_Animator.SetTrigger("GetHurt");
        }
        else if (state == eFSMStateID.WanderIdleStateID)
        {
            m_Animator.SetTrigger("WanderIdle");
        }
        else if (state == eFSMStateID.AttackStateID)
        {
            m_Animator.SetTrigger("Attack");
        }
        else if (state == eFSMStateID.CallArmyState)
        {
            m_Animator.SetTrigger("CallArmy");
        }
        else if (state == eFSMStateID.DeadStateID)
        {
            m_Animator.SetTrigger("Dead");
        }
    }

    private void UpdateAnimator(eFSMStateID state, bool Bool)
    {
        if (state == eFSMStateID.ChaseStateID)
        {
            m_Animator.SetBool("Chase", Bool);
        }
        else if (state == eFSMStateID.IdleStateID)
        {
            m_Animator.SetBool("Idle", Bool);
        }
        else if (state == eFSMStateID.WanderStateID)
        {
            m_Animator.SetBool("Wander", Bool);
        }
        else if (state == eFSMStateID.WanderIdleStateID)
        {
            m_Animator.SetBool("WanderIdle", Bool);
        }
        else if (state == eFSMStateID.FleeStateID)
        {
            m_Animator.SetBool("Flee", Bool);
        }
    }
}
