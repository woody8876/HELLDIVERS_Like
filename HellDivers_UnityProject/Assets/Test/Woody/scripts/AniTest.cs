﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AniTest : MonoBehaviour
{
    private float m_TurnAmount;
    private float m_ForwardAmount;
    private Animator m_Animator;
    private float m_BattleRight;
    private float m_BattleForward;

    private void Start()
    {
        m_Animator = this.GetComponent<Animator>();
    }
    public void Move(Vector3 move, Vector3 direction, bool run, bool inBattle, bool attack)
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
        UpdateAnimator(move, inBattle, attack);
    }

    void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(180f, 360f, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }
    void UpdateAnimator(Vector3 move, bool inBattle, bool attack)
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
            if(Vector3.Angle(this.transform.forward, move) > 20) m_Animator.SetBool("RotateStart", true);
            m_Animator.SetBool("WalkShoot", true);
            m_Animator.SetFloat("Turn", m_TurnAmount * 0.63f, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("WalkForward", m_BattleForward, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("WalkRight", m_BattleRight, 0.1f, Time.deltaTime);
        }
    }
}