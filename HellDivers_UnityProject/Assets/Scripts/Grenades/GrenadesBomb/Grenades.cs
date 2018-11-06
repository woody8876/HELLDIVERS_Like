﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenades : MonoBehaviour {


    [SerializeField] EGrenades m_Type;
    [SerializeField] protected int m_ID;
    [SerializeField] protected float m_fAngle = 30;

    public float m_Force
    {
        set
        {
            if (m_fForce > 15) m_fForce = 15;
            else m_fForce = value;
        }
        get { return m_fForce; }
    }

    public void Throw()
    {
        if (m_gEffect == null)
        {
            m_gEffect = ObjectPool.m_Instance.LoadGameObjectFromPool(m_ID + 100);
            m_gEffect.SetActive(true);
            m_vForward = transform.forward;
        }
        m_bGround = false;
    }

    #region Private function
    protected void Falling(float force)
    {
        var v0 = force * Mathf.Sin(m_fAngle * Mathf.Deg2Rad);
        var v = v0 + m_fGravity * m_fTime;
        transform.position += transform.up * v * Time.fixedDeltaTime;
    }
    protected void Moving(float force)
    {
        var vF = force * Mathf.Cos(m_fAngle * Mathf.Deg2Rad);
        transform.position += m_vForward * vF * Time.fixedDeltaTime;
        transform.Rotate(0, 30, 0);
    }
    protected void Damage(Vector3 pos)
    {
        var enemies = Physics.OverlapSphere(pos, grenadeInfo.Range, 1<<LayerMask.NameToLayer("Enemies"));
        foreach (var enemy in enemies)
        {
            IDamageable target = enemy.gameObject.GetComponent<IDamageable>();
            target.TakeDamage(grenadeInfo.Damage, enemy.transform.position);
        }
    }
    protected virtual bool GroundCheck()
    {
        RaycastHit rh;
        if (Physics.Raycast(transform.position, -transform.up, out rh, .5f, 1 << LayerMask.NameToLayer("Terrain")))
        {
            m_fTime = m_fForce = 0;
            
            m_gEffect.transform.position = rh.point - .3f * Vector3.up ;
            //DrawTools.DrawCircleSolid(this.transform, this.transform.position, grenadeInfo.Range);
            m_bCounting = true;
            return true;
        }
        return false;
    }
    protected virtual void CountDown()
    {
        m_fDamageTime += Time.fixedDeltaTime;
        if (m_fDamageTime <= Time.fixedDeltaTime) m_gEffect.GetComponent<Animator>().SetTrigger("startTrigger");
        if (m_fDamageTime > grenadeInfo.Timer)
        {
            m_fForce = 5;
            Damage(this.transform.position);
            m_fDamageTime = 0;
            ObjectPool.m_Instance.UnLoadObjectToPool(m_ID, this.gameObject);
            m_bCounting = false;
        }
    }
    #endregion

    #region MonoBehaviors
    protected void Start()
    {
        grenadeInfo = GameData.Instance.GrenadeInfoTable[m_ID];
    }
    protected void FixedUpdate()
    {
        if (!m_bGround )
        {
            Falling(m_fForce);
            Moving(m_fForce);
            m_fTime += Time.fixedDeltaTime;
            m_bGround = GroundCheck();
        }
        if (m_bCounting)
        {
            CountDown();
        }

    }
    #endregion

    #region Private Field
    protected float m_fForce = 5;
    protected float m_fGravity = -9.8f;
    protected float m_fTime;
    protected float m_fDamageTime;
    protected bool m_bGround = true;
    protected bool m_bCounting;
    protected GameObject m_gEffect;
    protected GrenadeInfo grenadeInfo;
    protected Vector3 m_vForward;
    #endregion
}