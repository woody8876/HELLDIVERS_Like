using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenades_Mine : Grenades {

    bool bEnd = false;

    protected override void CountDown()
    {
        RaycastHit rh;
        m_fDamageTime += Time.fixedDeltaTime;
        if (!bEnd)
        {
            if (m_fDamageTime <= Time.fixedDeltaTime)
            {
                this.GetComponent<MeshRenderer>().enabled = false;
                this.GetComponentInChildren<Animator>().SetTrigger("startTrigger");
            }
            if (Physics.SphereCast(transform.position - Vector3.up, 2, Vector3.up, out rh, 100, 1 << LayerMask.NameToLayer("Enemies")) || m_fDamageTime > grenadeInfo.Timer)
            {
                Debug.Log(rh.point);
                this.GetComponentInChildren<Animator>().SetTrigger("endTrigger");
                m_gEffect.GetComponent<Animator>().SetTrigger("startTrigger");
                m_fDamageTime = 0;
                bEnd = true;
            }
        }
        if (bEnd && m_fDamageTime > 0.4f)
        {
            Damage(this.transform.position);
            m_fDamageTime = 0;
            m_fForce = 10;
            ObjectPool.m_Instance.UnLoadObjectToPool(m_ID, this.gameObject);
            m_bCounting = false;
            bEnd = false;        
        }
    }
}
