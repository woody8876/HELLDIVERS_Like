using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamager
{
    float Damage { get; }
}

public interface IDamageable
{
    bool IsDead { get; }

    void TakeDamage(float dmg, Vector3 hitPoint);

    void TakeDamage(IDamager damager, Vector3 hitPoint);
}

public interface IHealable
{
    bool IsDead { get; }
    float CurrentHp { get; }
    float CurrentHpPercent { get; }

    bool GiveHealth(float heal);
}

public class Character : MonoBehaviour, IDamageable
{
    public bool IsDead { get { return m_bDead; } }

    public float CurrentHp
    {
        get { return m_CurrentHp; }
        set { m_CurrentHp = Mathf.Clamp(value, 0.0f, m_MaxHp); }
    }

    public float CurrentHpPercent { get { return m_CurrentHp / m_MaxHp; } }

    [SerializeField] protected float m_CurrentHp;
    [SerializeField] protected float m_MaxHp = 100;
    protected bool m_bDead;

    // Use this for initialization
    protected virtual void Start()
    {
        m_CurrentHp = m_MaxHp;
    }

    public virtual void TakeDamage(float dmg, Vector3 hitPoint)
    {
        if (IsDead) return;

        CurrentHp -= dmg;

        if (m_CurrentHp <= 0) Death();
    }

    public virtual void TakeDamage(IDamager damager, Vector3 hitPoint)
    {
        TakeDamage(damager.Damage, hitPoint);
    }

    public virtual void Death()
    {
        m_bDead = true;
        Destroy(this.gameObject);
    }
}