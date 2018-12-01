using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenades : MonoBehaviour{

    [SerializeField] EGrenades m_Type;
    [SerializeField] int m_ID;
    [SerializeField] protected float m_fAngle = 30;
    #region Private Field
    protected GrenadeInfo grenadeInfo;
    protected Vector3 m_vForward;
    protected bool m_bGround = true;
    protected bool m_bCounting;
    protected float m_fDamageTime;
    #endregion

    public void SetInfo(GrenadeInfo info)
    {
        grenadeInfo = info;
        m_bGround = false;
        m_vForward = transform.forward;
    }

    #region Private function
    protected virtual void Falling(float force)
    {
        var v0 = force * Mathf.Sin(m_fAngle * Mathf.Deg2Rad);
        var v = v0 + grenadeInfo.Gravity * grenadeInfo.FallingTime;
        transform.position += transform.up * v * Time.fixedDeltaTime;
    }

    protected virtual void Moving(float force)
    {
        var vF = force * Mathf.Cos(m_fAngle * Mathf.Deg2Rad);
        transform.position += m_vForward * vF * Time.fixedDeltaTime;
        transform.Rotate(0, 30, 0);
    }

    protected virtual void Damage(Vector3 pos)
    {
        var enemies = Physics.OverlapSphere(pos, grenadeInfo.Range, 1<<LayerMask.NameToLayer("Enemies") | 1<< LayerMask.NameToLayer("Player"));
        foreach (var enemy in enemies)
        {
            IDamageable target = enemy.gameObject.GetComponent<IDamageable>();
            target.TakeDamage(grenadeInfo.Damage, enemy.transform.position);
        }
    }

    protected virtual bool GroundCheck()
    {
        RaycastHit rh;
        if (Physics.Raycast(transform.position, -transform.up, out rh, .3f, 1 << LayerMask.NameToLayer("Terrain")))
        {
            grenadeInfo.FallingTime = grenadeInfo.Force = 0;

            transform.position = rh.point /*+ Vector3.down * .5f*/;

            m_bCounting = true;
            return true;
        }
        return false;
    }

    protected virtual void CountDown()
    {
        m_fDamageTime += Time.fixedDeltaTime;
        if (m_fDamageTime <= Time.fixedDeltaTime)
        {
            GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool(grenadeInfo.ID + 100);
            go.transform.position = transform.position;
            go.transform.forward = transform.forward;
            go.SetActive(true);
        }
        if (m_fDamageTime > grenadeInfo.Timer)
        {
            grenadeInfo.Force = 1;
            Damage(this.transform.position);
            m_fDamageTime = 0;
            ObjectPool.m_Instance.UnLoadObjectToPool(grenadeInfo.ID, this.gameObject);
            m_bCounting = false;
        }
    }

    #endregion
    protected virtual void FixedUpdate()
    {
        if (!m_bGround)
        {
            Falling(grenadeInfo.Force);
            Moving(grenadeInfo.Force);
            grenadeInfo.FallingTime += Time.fixedDeltaTime;
            m_bGround = GroundCheck();
        }
        if (m_bCounting)
        {
            CountDown();
        }
    }

    #region MonoBehaviors
    #endregion

}
