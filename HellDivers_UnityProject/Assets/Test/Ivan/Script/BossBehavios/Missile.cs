using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    public float m_fRadius;
    public float m_fGravity;

    float m_fSqrtRadius;

	// Use this for initialization
	void Start () {
        m_fSqrtRadius = m_fRadius * m_fRadius;
	}
	
	// Update is called once per frame
	void Update () {

        if(!GroundCheck(transform)) Falling(m_fGravity);

    }

    public void Falling(float g)
    {
        Vector3 pos = transform.position;
        pos.y -= Time.deltaTime * g;
        transform.position = pos;
    }

    public bool GroundCheck(Transform t)
    {
        Vector3 pos = t.position;
        float sqrtHeight = pos.y * pos.y;
        if (sqrtHeight <= m_fSqrtRadius)
        {
            pos.y = 1;
            t.position = pos;
            if (gameObject.name == "Missile(Clone)")
                ObjectPool.m_Instance.UnLoadObjectToPool((int)EItem.MISSILE, this.gameObject);
            return true;
        }
        return false;
    }

}
