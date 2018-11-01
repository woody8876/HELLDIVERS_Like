using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenades : MonoBehaviour {

    [SerializeField] float m_fAngle;
    [SerializeField] float m_fForce;
    float m_fGravity = -9.8f;
    float m_fTime;
         

	// Use this for initialization
	void Start () {
		
	}

    private void FixedUpdate()
    {
        if (!GroundCheck())
        {
            Falling(m_fForce);
            Moving(m_fForce);
            m_fTime += Time.fixedDeltaTime;
        }
        else
        {
            m_fTime = 0;
        }

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
        if (transform.position.y <= 2) return true;
        return false;
    }
}
