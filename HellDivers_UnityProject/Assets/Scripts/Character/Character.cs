using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    #region Properties

    /// <summary>
    /// Was the character already dead.
    /// </summary>
    public bool IsDead { get { return m_bDead; } }

    /// <summary>
    /// Represent the current health point.
    /// The value will clamp by ( 0 - max health point )
    /// </summary>
    public float CurrentHp
    {
        get { return m_CurrentHp; }
        set { m_CurrentHp = Mathf.Clamp(value, 0.0f, m_MaxHp); }
    }

    /// <summary>
    /// Represent the current health point by max health point. ( 0 - 1 )
    /// </summary>
    public float CurrentHpPercent { get { return m_CurrentHp / m_MaxHp; } }

    #endregion Properties

    #region Private Variable

    [SerializeField] protected float m_CurrentHp;
    [SerializeField] protected float m_MaxHp = 100;
    protected bool m_bDead;

    #endregion Private Variable

    #region MonoBehaviour

    /// <summary>
    /// Set current health point = max health point.
    /// </summary>
    protected virtual void Start()
    {
        m_CurrentHp = m_MaxHp;
    }

    #endregion MonoBehaviour

    #region Public Function

    /// <summary>
    /// Send damage point for decrease current health point.
    /// </summary>
    /// <param name="dmg">Damage point</param>
    /// <param name="hitPoint">Hit point position</param>
    /// <returns>Was the current health point decreased or not ?</returns>
    public virtual bool TakeDamage(float dmg, Vector3 hitPoint)
    {
        if (IsDead) return false;

        CurrentHp -= dmg;
        if (m_CurrentHp <= 0) Death();
        return true;
    }

    /// <summary>
    /// Send damage point for decrease current health point.
    /// </summary>
    /// <param name="damager">The damager</param>
    /// <param name="hitPoint">Hit point position</param>
    /// <returns>Was the current health point decreased or not ?</returns>
    public virtual bool TakeDamage(IDamager damager, Vector3 hitPoint)
    {
        return TakeDamage(damager.Damage, hitPoint);
    }

    public virtual void Death()
    {
        m_bDead = true;
        Destroy(this.gameObject);
    }

    #endregion Public Function
}