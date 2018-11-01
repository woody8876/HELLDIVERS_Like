using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenades : MonoBehaviour {

    [SerializeField] float m_fAngle;
    [SerializeField] float m_fForce;
    float m_fGravity = -9.8f;
    float m_fTime;
    bool m_bGround = true;

	// Use this for initialization
	void Start () {
		
	}

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            m_fForce += 2 * Time.fixedDeltaTime;
            transform.position = Input.mousePosition;
            m_bGround = false;
        }
        else if (Input.GetMouseButtonUp(0)) Throw();
        if (!m_bGround)
        {
            Falling(m_fForce);
            Moving(m_fForce);
            m_fTime += Time.fixedDeltaTime;
            m_bGround = GroundCheck();
        }


    }

    void Throw()
    {
        m_bGround = false;
    }

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
        if (transform.position.y <= 2)
        {
            m_fTime = m_fForce = 0;
            return true;
        }
        return false;
    }
}
