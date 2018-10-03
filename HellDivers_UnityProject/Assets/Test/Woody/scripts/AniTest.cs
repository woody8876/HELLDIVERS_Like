using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AniTest : MonoBehaviour
{
    private float m_TurnAmount;
    private float m_ForwardAmount;
    private Animator m_Animator;
    private void Start()
    {
        m_Animator = this.GetComponent<Animator>();
    }

    public void Move(Vector3 move)
    {
        move = transform.InverseTransformDirection(move);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;
        ApplyExtraTurnRotation();
        UpdateAnimator(move);
    }

    void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(180f, 360f, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }
    void UpdateAnimator(Vector3 move)
    {
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount/1.57f, 0.1f, Time.deltaTime);
    }
}