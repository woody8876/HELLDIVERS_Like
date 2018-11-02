using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenades : MonoBehaviour {


    [SerializeField] EGrenades m_Type;
    [SerializeField] float m_fAngle = 30;

    public float m_Force
    {
        set
        {
            if (m_fForce > 20) m_fForce = 20;
            else m_fForce = value;
        }
        get { return m_fForce; }
    }
    public void Throw()
    {
        if (m_gEffect == null)
        {
            m_gEffect = ObjectPool.m_Instance.LoadGameObjectFromPool(3011);
            m_gEffect.SetActive(true);
        }
        m_bGround = false;
    }
    #region Private function
    void Falling(float force)
    {
        var v0 = force * Mathf.Sin(m_fAngle * Mathf.Deg2Rad);
        var v = v0 + m_fGravity * m_fTime;
        transform.position += transform.up * v * Time.fixedDeltaTime;
    }
    void Moving(float force)
    {
        var vF = force * Mathf.Cos(m_fAngle * Mathf.Deg2Rad);
        transform.position += transform.forward * vF * Time.fixedDeltaTime;
    }
    void Damage(Vector3 pos)
    {
        var enemies = Physics.OverlapSphere(pos, 10, 1<<LayerMask.NameToLayer("Enemies"));
        foreach (var enemy in enemies)
        {
            IDamageable target = enemy.gameObject.GetComponent<IDamageable>();
            target.TakeDamage(80, enemy.transform.position);
        }
    }

    bool GroundCheck()
    {
        RaycastHit rh;
        if (Physics.Raycast(transform.position, -transform.up, out rh, .5f, 1 << LayerMask.NameToLayer("Terrain")))
        {
            m_fDamageTime += Time.fixedDeltaTime;
            m_fTime = m_fForce = 0;
            m_gEffect.transform.position = rh.point + Vector3.up;
            if (m_fDamageTime <= Time.fixedDeltaTime) m_gEffect.GetComponent<Animator>().SetTrigger("startTrigger");
            if (m_fDamageTime > 0.4f)
            {
                m_fForce = 10;
                Damage(rh.point);
                m_fDamageTime = 0;
                ObjectPool.m_Instance.UnLoadObjectToPool(3001, this.gameObject);
                return true;
            }
        }
        return false;
    }
    #endregion

    #region MonoBehaviors
    private void FixedUpdate()
    {
        if (!m_bGround )
        {
            Falling(m_fForce);
            Moving(m_fForce);
            m_fTime += Time.fixedDeltaTime;
            m_bGround = GroundCheck();
        }

    }
    #endregion
    #region Private Field
    float m_fForce = 10;
    float m_fGravity = -9.8f;
    float m_fTime;
    float m_fDamageTime;
    bool m_bGround = true;
    GameObject m_gEffect;
    #endregion
}
