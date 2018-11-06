using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour {

    [SerializeField] int m_ID;

    Animator m_Animator;
    float m_fTime;
    bool m_bFinish;

    private void EffectStart()
    {
        m_Animator.SetTrigger("start");
    }
    private bool CheckState()
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("end")) return true;
        else return false;
    }

    private void UnLoadObject()
    {
        ObjectPool.m_Instance.UnLoadObjectToPool(m_ID, this.gameObject);
    }

	// Use this for initialization
	void Awake () {
        m_Animator = GetComponent<Animator>();
	}
    
    // Update is called once per frame
    void Update () {
        if (CheckState()) { m_bFinish = true; }
		if (m_bFinish)
        {
            m_bFinish = false;
            UnLoadObject();
        }
	}
}
