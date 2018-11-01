using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenades : MonoBehaviour {

    [SerializeField]public float m_fAngle;
    private float m_fForce;
    public float  m_Force
    {
        set
        {
            if (m_fForce > 20) m_fForce = 20;
            else m_fForce = value;
        }
        get { return m_fForce; }
    }

    float m_fGravity = -9.8f;
    float m_fTime;
    bool m_bGround = true;
	// Use this for initialization
	void Start () {
		
	}

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

    public void Throw() { m_bGround = false; }

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
         
    bool GroundCheck()
    {
        if (Physics.Raycast(transform.position, -transform.up, .5f, 1 << LayerMask.NameToLayer("Terrain")))
        {
            m_fTime = 0;
            m_fForce = 10;
            ObjectPool.m_Instance.UnLoadObjectToPool(3001, this.gameObject);
            return true;
        }
        return false;
    }
}
