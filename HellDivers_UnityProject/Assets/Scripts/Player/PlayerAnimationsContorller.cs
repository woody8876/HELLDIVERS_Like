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

    public void ResetAnimator(PlayerController data)
    {
        m_Animator.ResetTrigger("Reset");
    }
    /// <summary>
    /// Set Trigger Animator
    /// </summary>
    /// <param name="state"></param>
    /// 
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

    public void SetAnimator(ePlayerFSMStateID state, float speed)
    {
        UpdateAnimator(state, speed);
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
            //if (Vector3.Angle(this.transform.forward, move) > 20)
            //{
            //    m_Animator.SetBool("RotateStart", true);
            //    //m_Animator.Play("Rotate", 0, 0);
            //}
            m_Animator.SetBool("WalkShoot", true);
            m_Animator.SetFloat("Turn", m_TurnAmount * 0.63f, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("WalkForward", m_BattleForward/*, 0.1f, Time.deltaTime*/);
            m_Animator.SetFloat("WalkRight", m_BattleRight/*, 0.1f, Time.deltaTime*/);
        }
    }

    private void UpdateAnimator(ePlayerFSMStateID state)
    {
        switch (state)
        {
            case ePlayerFSMStateID.GunStateID:
                m_Animator.SetTrigger("Shoot");
                break;
            case ePlayerFSMStateID.ReloadStateID:
                m_Animator.SetTrigger("Reload");
                break;
            case ePlayerFSMStateID.MeleeAttackID:
                m_Animator.SetTrigger("MeleeAttack");
                break;
            case ePlayerFSMStateID.SwitchWeaponID:
                m_Animator.SetTrigger("SwitchWeapon");
                break;
            case ePlayerFSMStateID.StratagemStateID:
                m_Animator.SetTrigger("ThrowStandby");
                break;
            case ePlayerFSMStateID.ThrowBombStateID:
                m_Animator.SetTrigger("BombReady");
                break;
            case ePlayerFSMStateID.DeadStateID:
                m_Animator.SetTrigger("Death");
                m_Animator.SetTrigger("Reset");
                break;
            case ePlayerFSMStateID.VictoryID:
                m_Animator.SetTrigger("Victory");
                break;
            case ePlayerFSMStateID.ReliveStateID:
                m_Animator.SetTrigger("Relive");
                break;
            case ePlayerFSMStateID.PickUpID:
                m_Animator.SetTrigger("Pick");
                break;
            case ePlayerFSMStateID.RollStateID:
                m_Animator.SetTrigger("Roll");
                break;
        }
    }

    private void UpdateAnimator(ePlayerFSMStateID state, bool Bool)
    {
        switch (state)
        {
            case ePlayerFSMStateID.GunStateID:
                m_Animator.SetBool("Shoot", Bool);
                break;
            case ePlayerFSMStateID.StratagemStateID:
                m_Animator.SetBool("InputCodes", Bool);
                break;
            case ePlayerFSMStateID.ThrowStateID:
                m_Animator.SetBool("ThrowOut", Bool);
                break;
            case ePlayerFSMStateID.ThrowBombStateID:
                m_Animator.SetBool("ThrowBomb", Bool);
                break;
        }
    }

    private void UpdateAnimator(ePlayerFSMStateID state, float speed)
    {
        if (state == ePlayerFSMStateID.ReloadStateID)
        {
            m_Animator.SetTrigger("Reload");
            m_Animator.SetFloat("ReloadSpeed", speed);
        }
    }
}