using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBomb : MonoBehaviour {

    private float m_Timer = 0.0f;
    private Animator m_Animator;
    // Use this for initialization
    private void OnEnable()
    {
        if(m_Animator == null) m_Animator = this.GetComponent<Animator>();
        m_Animator.SetTrigger("startTrigger");
        m_Timer = 0.0f;
        List<Player> pList = InGamePlayerManager.Instance.Players;
        if (pList != null && pList.Count > 0)
        {
            for (int i = 0; i < pList.Count; i++)
            {
                if (pList[i].IsDead) continue;
                float Dist = (pList[i].transform.position - this.transform.position).magnitude;
                if (Dist <= 5.0f)
                {
                    IDamageable target = pList[i].transform.gameObject.GetComponent<IDamageable>();
                    target.TakeDamage(10.0f, this.transform.position);
                }
            }
        }
    }
    void Start()
    {
        m_Animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer > 2.0f)
        {
            ObjectPool.m_Instance.UnLoadObjectToPool(3403, this.gameObject);
        }
    }
}
