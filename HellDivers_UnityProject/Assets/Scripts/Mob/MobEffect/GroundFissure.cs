using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFissure : MonoBehaviour {

    private float m_Timer = 0.0f;
    // Use this for initialization
    private void OnEnable()
    {
        m_Timer = 0.0f;
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        m_Timer += Time.deltaTime;
        if (m_Timer > 2.0f)
        {
            ObjectPool.m_Instance.UnLoadObjectToPool(3401, this.gameObject);
        }
	}
}
