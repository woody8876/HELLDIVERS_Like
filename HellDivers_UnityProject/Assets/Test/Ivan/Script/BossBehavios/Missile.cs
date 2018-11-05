using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    Transform m_tTarget;
    float m_fForce;
    float m_fTime;
    bool m_bCalculate = true;
    // Update is called once per frame
    private void Start() {
    }

    

    private void FixedUpdate()
    {
        if (!GroundCheck(transform))
        {
            CalculateForce();
            Falling();
            ForwardMoving(m_fForce);
        }
    }

    private void Falling()
    {
        Vector3 pos = transform.position;
        float v = -9.8f * m_fTime;
        pos.y += v * Time.fixedDeltaTime;  
        transform.position = pos;
        m_fTime += Time.fixedDeltaTime;
    }

    private void ForwardMoving(float g)
    {
        transform.position += transform.forward * g * Time.fixedDeltaTime;
    }

    private bool GroundCheck(Transform t)
    {
        Vector3 pos = t.position;
        if (pos.y <= 0.5f)
        {
            m_fTime = 0;
            pos.y = 0;
            t.position = pos;
            m_bCalculate = true;
            if (gameObject.name == "Missile(Clone)")
                ObjectPool.m_Instance.UnLoadObjectToPool((int)BossStateFuntion.EItem.MISSILE, this.gameObject);
            return true;
        }
        return false;
    }

    public void CalculateForce()
    {
        if (!m_bCalculate) { return; }
        m_tTarget = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 target = m_tTarget.position;
        Vector3 pos = transform.position;
        target.y = 0;
        pos.y = 0;
        float length = (pos - target).sqrMagnitude;
        float t = transform.position.y * 0.204f;
        m_fForce = Mathf.Sqrt(length / t);
        m_bCalculate = false;
    }
}
