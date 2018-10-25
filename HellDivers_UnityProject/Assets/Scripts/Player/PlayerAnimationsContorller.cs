using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsContorller : MonoBehaviour
{
    public Animator Animator { get { return m_Animator; } set { m_Animator = value; } }

    private Animator m_Animator;
    private float m_TurnAmount;
    private float m_ForwardAmount;
    private float m_BattleRight;
    private float m_BattleForward;

    private void Awake()
    {
        m_Animator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        
    }
    public void Move(Vector3 move, Vector3 direction, bool run, bool inBattle)
    {
        if (!inBattle)
        {
            move = transform.InverseTransformDirection(move);
            m_ForwardAmount = move.z;
            m_TurnAmount = Mathf.Atan2(move.x, move.z);
            if (run) m_ForwardAmount *= 2;
        }
        else if (inBattle)
        {
            Vector3 currentMove = direction;
            direction = transform.InverseTransformDirection(direction);
            m_TurnAmount = Mathf.Atan2(direction.x, direction.z);
            m_BattleRight = Vector3.Dot(this.transform.right, move.normalized);
            m_BattleForward = Vector3.Dot(this.transform.forward, move.normalized);
            move = currentMove;
        }
        ApplyExtraTurnRotation();

        if (m_Animator != null)
        {
            UpdateAnimator(move, inBattle);
        }
    }

    private void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(180f, 360f, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * 3f * Time.deltaTime, 0);
    }


    public bool FinishAnimator(PlayerFSMData data)
    {
        data.m_FinishAni = false;
        AnimatorStateInfo info = m_Animator.GetCurrentAnimatorStateInfo(1);
        data.m_FinishAni = (info.normalizedTime >= 0.9f) ? true : false;
        return data.m_FinishAni;
    }  

    /// <summary>
    /// Set Trigger Animator
    /// </summary>
    /// <param name="state"></param>
    public void SetAnimator(ePlayerFSMStateID state)
    {
        UpdateAnimator(state);
    }

    /// <summary>
    /// Set Bool Animator
    /// </summary>
    /// <param name="state"></param>
    /// <param name="Bool"></param>
    /// 
    public void SetAnimator(ePlayerFSMStateID state, bool Bool)
    {
        UpdateAnimator(state, Bool);
    }

    private void UpdateAnimator(Vector3 move, bool inBattle)
    {
        if (!inBattle)
        {
            m_Animator.SetBool("WalkShoot", false);
            m_Animator.SetBool("RotateStart", false);
            m_Animator.SetFloat("Turn", m_TurnAmount * 0.63f, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        }
        else if (inBattle)
        {
            if (Vector3.Angle(this.transform.forward, move) > 20) m_Animator.SetBool("RotateStart", true);
            m_Animator.SetBool("WalkShoot", true);
            m_Animator.SetFloat("Turn", m_TurnAmount * 0.63f, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("WalkForward", m_BattleForward, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("WalkRight", m_BattleRight, 0.1f, Time.deltaTime);
        }
    }

    private void UpdateAnimator(ePlayerFSMStateID state)
    {
        if (state == ePlayerFSMStateID.ReloadStateID)
        {
            m_Animator.SetTrigger("Reload");
        }
        else if (state == ePlayerFSMStateID.SwitchWeaponID)
        {
            m_Animator.SetTrigger("SwitchWeapon");
        }
        else if (state == ePlayerFSMStateID.StratagemStateID)
        {
            m_Animator.SetTrigger("ThrowStandby");
        }
        else if (state == ePlayerFSMStateID.ThrowStateID)
        {
            m_Animator.SetTrigger("ThrowOut");
        }
        else if (state == ePlayerFSMStateID.DeadStateID)
        {
            m_Animator.SetTrigger("Death");
        }
    }

    private void UpdateAnimator(ePlayerFSMStateID state, bool Bool)
    {
        if (state == ePlayerFSMStateID.GunStateID)
        {
            m_Animator.SetBool("Shoot", Bool);
        }
        else if (state == ePlayerFSMStateID.StratagemStateID)
        {
            m_Animator.SetBool("InputCodes", Bool);
        }
        else if (state == ePlayerFSMStateID.ThrowStateID)
        {
            m_Animator.SetBool("ThrowOut", Bool);
        }
        else if (state == ePlayerFSMStateID.DeadStateID)
        {
            m_Animator.SetBool("Relive", Bool);
        }
        else if (state == ePlayerFSMStateID.PickUpID)
        {
            m_Animator.SetBool("Pick", Bool);
        }
    }

}