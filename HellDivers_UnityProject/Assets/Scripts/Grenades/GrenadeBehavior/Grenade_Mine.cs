using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade_Mine : Grenades
{
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
            if (Physics.SphereCast(transform.position - Vector3.up, 2, Vector3.up, out rh, 100, 1 << LayerMask.NameToLayer("Enemies") | 1 << LayerMask.NameToLayer("Player")) || m_fDamageTime > grenadeInfo.Timer)
            {
                Debug.Log(rh.point);
                this.GetComponentInChildren<Animator>().SetTrigger("endTrigger");
                GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool(grenadeInfo.ID + 100);
                go.transform.position = this.transform.position;
                go.transform.forward = this.transform.forward;
                m_fDamageTime = 0;
                bEnd = true;
            }
        }
        if (bEnd && m_fDamageTime > 0.4f)
        {
            DoDamage(this.transform.position);
            m_fDamageTime = 0;
            grenadeInfo.Force = 10;
            ObjectPool.m_Instance.UnLoadObjectToPool(grenadeInfo.ID, this.gameObject);
            m_bCounting = false;
            bEnd = false;
        }
    }

}
