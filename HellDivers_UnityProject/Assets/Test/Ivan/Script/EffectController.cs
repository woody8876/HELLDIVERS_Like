using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour {

    [SerializeField] int m_ID;

    Animator m_Animator;
    float m_fTime;
    bool m_bFinish;


    private void UnLoadObject()
    {
        ObjectPool.m_Instance.UnLoadObjectToPool(m_ID, this.gameObject);
    }

	// Use this for initialization
	void Start () {
        m_Animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_bFinish)
        {
            m_bFinish = false;
            UnLoadObject();
        }
	}
}
